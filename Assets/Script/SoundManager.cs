using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clips")]
    // 오디오 클립들을 이름을 키(Key)로 관리하기 위한 리스트 (인스펙터 세팅용)
    [SerializeField] private List<SoundData> soundEffects;
    [SerializeField] private List<SoundData> bgmClips;

    private Dictionary<string, AudioClip> sfxDictionary = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> bgmDictionary = new Dictionary<string, AudioClip>();

    [System.Serializable]
    public struct SoundData
    {
        public string name;
        public AudioClip clip;
    }

    private void Awake()
    {
        // 싱글톤 구조 세팅
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 파괴되지 않음
            InitDictionaries();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 리스트로 받은 오디오 클립들을 딕셔너리로 변환해 검색 속도 최적화
    private void InitDictionaries()
    {
        foreach (var sound in soundEffects) sfxDictionary[sound.name] = sound.clip;
        foreach (var sound in bgmClips) bgmDictionary[sound.name] = sound.clip;
    }

    #region 배경음(BGM) 제어
    public void PlayBGM(string name, bool loop = true)
    {
        if (!bgmDictionary.TryGetValue(name, out AudioClip clip))
        {
            Debug.LogWarning($"BGM을 찾을 수 없습니다: {name}");
            return;
        }

        if (bgmSource.clip == clip) return; // 이미 재생 중인 BGM이면 무시

        bgmSource.clip = clip;
        bgmSource.loop = loop;
        bgmSource.Play();
    }

    public void StopBGM() => bgmSource.Stop();
    public void SetBGMVolume(float volume) => bgmSource.volume = Mathf.Clamp01(volume);
    #endregion

    #region 효과음(SFX) 제어
    public void PlaySFX(string name, float volume = 1f)
    {
        if (sfxDictionary.TryGetValue(name, out AudioClip clip))
        {
            // PlayOneShot은 여러 효과음이 겹쳐서 재생될 수 있게 해줍니다.
            sfxSource.PlayOneShot(clip, volume);
        }
        else
        {
            Debug.LogWarning($"SFX를 찾을 수 없습니다: {name}");
        }
    }

    public void SetSFXVolume(float volume) => sfxSource.volume = Mathf.Clamp01(volume);
    #endregion
}
