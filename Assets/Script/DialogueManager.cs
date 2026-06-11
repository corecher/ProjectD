using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("UI 요소")]
    public Text dialogueText;       // 대사가 출력될 레거시 Text 컴포넌트

    [Header("대사 데이터")]
    public List<DialogueData> sentences;          // 출력할 대사들을 저장하는 배열

    [Header("설정")]
    public float typingSpeed = 0.05f;   // 글자가 타이핑되는 속도

    [Header("씬 전환 설정")]
    public Image fadeImage;             // 페이드 효과에 사용할 UI Image
    private int currentIndex = 0;       // 현재 대사 번호
    private bool isTyping = false;      // 현재 글자가 타이핑 중인지 여부
    private bool isEnding = false;      // 대사가 끝나고 씬 전환 중인지 체크
    private Coroutine typingCoroutine;  // 타이핑 코루틴 제어용
    private int i;
    void Start()
    {
        i=FadeManager.Instance.floor-2;
        if (fadeImage != null)
        {
            Color color = fadeImage.color;
            color.a = 0f;
            fadeImage.color = color;
            fadeImage.gameObject.SetActive(false);
        }
        StartDialogue();
    }

    void Update()
    {
        if (isEnding) return;

        // 마우스 좌클릭 또는 스페이스바를 눌렀을 때
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                // 1. 글자가 나오는 중이었다면 -> 즉시 전체 대사 출력
                FinishSentenceEarly();
            }
            else
            {
                // 2. 글자 출력이 끝난 상태였다면 -> 다음 대사로 진행
                DisplayNextSentence();
            }
        }
    }

    // 대사 시스템 시작
    public void StartDialogue()
    {
        currentIndex = 0;
        DisplayNextSentence();
    }

    // 다음 대사 넘기기
    public void DisplayNextSentence()
    {
        // 모든 대사가 끝났다면
        if (currentIndex >= sentences[i].dialogue.Length)
        {
            EndDialogue();
            return;
        }

        // 기존에 돌고 있던 타이핑 코루틴이 있다면 멈춤
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        // 새 대사 타이핑 시작
        typingCoroutine = StartCoroutine(TypeSentence(sentences[i].dialogue[currentIndex]));
        currentIndex++;
    }

    // 한 글자씩 출력하는 코루틴
    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        isTyping = true;

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed); // 설정한 속도만큼 대기
        }

        isTyping = false;
    }

    // 대사 즉시 완성하기
    void FinishSentenceEarly()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        
        // 현재 인덱스가 이미 증가했으므로 -1 해줌
        dialogueText.text = sentences[i].dialogue[currentIndex - 1]; 
        isTyping = false;
    }

    // 모든 대사가 종료되었을 때 호출
    void EndDialogue()
    {
        isEnding = true;
        dialogueText.text = "";
        StartCoroutine(FadeManager.Instance.FadeOutAndLoadScene(100));
    }
    
}
