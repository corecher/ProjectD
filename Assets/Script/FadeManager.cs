using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;
    [Header("UI 요소")]
    public Image fadeImage;      // 화면을 덮을 검은색 이미지
    public float fadeDuration = 1.0f;
    public List<string> nextSceneNames;
    public int index;
    public int floor = 2;
    [SerializeField]private GameObject EscPanel;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(SceneLoadedRoutine(scene));
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        ESC();
    }
    public void ESC()
    {
        Time.timeScale = EscPanel.activeSelf?1f:0f;
        EscPanel.SetActive(!EscPanel.activeSelf);
    }
    public void ContinueGame()
    {
        EscPanel.SetActive(false);
        Time.timeScale = 1f;
    }
    private IEnumerator SceneLoadedRoutine(Scene scene)
    {
        yield return null;
        if (scene.name != "GameEndScene")
        {
            fadeImage.color = Color.black;
        }
        yield return StartCoroutine(FadeIn());
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public IEnumerator FadeIn()
    {
        if (fadeImage == null) yield break;

        fadeImage.gameObject.SetActive(true);
        
        Color color = fadeImage.color;
        color.a = 1f;
        fadeImage.color = color;

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            // 1에서 0으로 투명도 감소
            float alpha = Mathf.Clamp01(1f - (elapsedTime / fadeDuration));
            
            color.a = alpha;
            fadeImage.color = color;

            yield return null;
        }

        // 투명해진 후 뒤에 있는 게임 버튼 등을 누를 수 있게 반드시 비활성화
        fadeImage.gameObject.SetActive(false);
    }
    public IEnumerator FadeOutAndLoadScene(int i = 0 , int success = 3)
    { 
        string nextSceneName;
        if(success == 0) 
        {
            floor++;
            if(floor>=nextSceneNames.Count-1)
            floor--;
        }
        else if (success == 1)
        {
            floor--;
            if(floor<2) floor ++;
        }
        Debug.Log(index);
        if(i==100) nextSceneName = nextSceneNames[floor];
        else nextSceneName = nextSceneNames[i];
        
        if (fadeImage == null)
        {
            Debug.LogError("Fade Image가 연결되지 않았습니다! 바로 씬을 전환합니다.");
            SceneManager.LoadScene(nextSceneName);
            yield break;
        }

        fadeImage.gameObject.SetActive(true);
        Color startColor = fadeImage.color;
        startColor.a = 0f;
        fadeImage.color = startColor;

        float elapsedTime = 0f;

        // 투명도(Alpha)를 0에서 1로 서서히 증가시킴
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / 1f);
            
            Color color = fadeImage.color;
            color.a = alpha;
            fadeImage.color = color;

            yield return null;
        }

        // 페이드 완료 후 완전히 어두워졌을 때 다음 씬 로드
        SceneManager.LoadScene(nextSceneName);
    }
}
