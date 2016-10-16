using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour 
{

	#region Singleton
	static private SoundManager _instance;
	static public SoundManager GetInstance()
	{
		if(_instance == null)
		{
			_instance = new GameObject("SoundManager", typeof(SoundManager)).GetComponent<SoundManager>();
			_instance.LoadSounds ();
			GameObject.DontDestroyOnLoad(_instance);;
		}	
		return _instance; 
	}
	#endregion

	//Audio Sources
	public AudioSource			bgmAudioSource;
	public List<AudioSource>	sfxAudioSources = new List<AudioSource>();
	//Control
	public int					sfxSources = 5;
	public int					sfxCounter = 0;

	//Audio Clips
	public AudioClip			bgmClip;
	public AudioClip			movimentClip;
	public AudioClip			helloClip;
	public AudioClip			excuseMeClip;
	public AudioClip			clickClip;
	public AudioClip			endOfLevelClip;
	public AudioClip			defeatClip;
	public AudioClip			errorClip;
	public List<AudioClip>		enemyYawnClips = new List<AudioClip>();	
	public List<AudioClip>		playerYawnClips = new List<AudioClip>();
		
	private void LoadSounds()
	{
		bgmAudioSource = new GameObject("BGMAudioSource", typeof(AudioSource)).GetComponent<AudioSource>();
		bgmAudioSource.transform.parent = _instance.transform;
		for (int i = 0; i < sfxSources; i ++) 
		{
			sfxAudioSources.Add(new GameObject ("SFXAudioSource" + i.ToString(), typeof(AudioSource)).GetComponent<AudioSource> ());
			sfxAudioSources[sfxAudioSources.Count -1].gameObject.transform.parent = _instance.transform;
		}
		bgmClip = Resources.Load<AudioClip> ("Audio/Music/Yawn");
		movimentClip = Resources.Load<AudioClip> ("Audio/SFX/Moviment");
		helloClip = Resources.Load<AudioClip> ("Audio/SFX/Hello");
		excuseMeClip = Resources.Load<AudioClip> ("Audio/SFX/ExcuseMe");
		clickClip = Resources.Load<AudioClip> ("Audio/SFX/Click");
		endOfLevelClip = Resources.Load<AudioClip> ("Audio/SFX/EndOfLevel");
		defeatClip = Resources.Load<AudioClip> ("Audio/SFX/Defeat");
		errorClip = Resources.Load<AudioClip> ("Audio/SFX/Error");
		enemyYawnClips.Add(Resources.Load<AudioClip> ("Audio/SFX/EnemyYawn_1"));
		enemyYawnClips.Add(Resources.Load<AudioClip> ("Audio/SFX/EnemyYawn_2"));
		playerYawnClips.Add(Resources.Load<AudioClip> ("Audio/SFX/PlayerYawn_1"));
		playerYawnClips.Add(Resources.Load<AudioClip> ("Audio/SFX/PlayerYawn_2"));
	}
	public void PlayBGM()
	{
		if (bgmAudioSource.isPlaying)
			return;
		bgmAudioSource.clip = bgmClip;
		bgmAudioSource.loop = true;
		bgmAudioSource.volume = 0.4f;
		bgmAudioSource.Play ();
	}
	public void PlayMovimentSFX(float p_pitch = 1f)
	{
		sfxAudioSources [sfxCounter].volume = 1f;
		sfxAudioSources [sfxCounter].clip = movimentClip;
        IncreaseSFXCounter (p_pitch);
	}
	public void PlayHelloSFX(float p_pitch = 1f)
	{
		sfxAudioSources [sfxCounter].volume = 1f;
		sfxAudioSources [sfxCounter].clip = helloClip;
        IncreaseSFXCounter (p_pitch);
	}
	public void PlayExcuseMeSFX(float p_pitch = 1f)
	{
		sfxAudioSources [sfxCounter].volume = 1f;
		sfxAudioSources [sfxCounter].clip = excuseMeClip;
        IncreaseSFXCounter (p_pitch);
	}
	public void PlayClickSFX(float p_pitch = 1f)
	{
		sfxAudioSources [sfxCounter].volume = 0.8f;
		sfxAudioSources [sfxCounter].clip = clickClip;
        IncreaseSFXCounter (p_pitch);
	}
	public void PlayEndOfLevelSFX(float p_pitch = 1f)
	{
        sfxAudioSources[sfxCounter].volume = p_pitch < 1f ? 0.5f : 1f ;
		sfxAudioSources [sfxCounter].clip = endOfLevelClip;
        IncreaseSFXCounter (p_pitch);
	}
	public void PlayDefeatSFX(float p_pitch = 1f)
	{
		sfxAudioSources [sfxCounter].volume = 0.4f;
		sfxAudioSources [sfxCounter].clip = defeatClip;
        IncreaseSFXCounter (p_pitch);
	}
	public void PlayErrorSFX(float p_pitch = 1f)
	{
		sfxAudioSources [sfxCounter].volume = 0.2f;
		sfxAudioSources [sfxCounter].clip = errorClip;
        IncreaseSFXCounter (p_pitch);
	}
	public void PlayEnemyYawnSFX(float p_pitch = 1f)
	{
		sfxAudioSources [sfxCounter].volume = 0.5f;
		sfxAudioSources [sfxCounter].clip = enemyYawnClips[Random.Range(0,2)];
        IncreaseSFXCounter (p_pitch);
	}
	public void PlayPlayerYawnSFX(float p_pitch = 1f)
	{
		sfxAudioSources [sfxCounter].volume = 1f;
		sfxAudioSources [sfxCounter].clip = playerYawnClips[Random.Range(0,2)];
        IncreaseSFXCounter (p_pitch);
	}
	private void IncreaseSFXCounter(float p_pitch = 1f)
	{
        sfxAudioSources[sfxCounter].pitch = p_pitch;
        sfxAudioSources [sfxCounter].Play ();
		sfxCounter ++;
		if (sfxCounter == sfxAudioSources.Count)
			sfxCounter = 0;
	}
}
