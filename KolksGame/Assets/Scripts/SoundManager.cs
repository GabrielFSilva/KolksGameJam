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
		bgmAudioSource.volume = 0.5f;
		bgmAudioSource.Play ();
	}
	public void PlayMovimentSFX()
	{
		sfxAudioSources [sfxCounter].clip = movimentClip;
		IncreaseSFXCounter ();
	}
	public void PlayHelloSFX()
	{
		sfxAudioSources [sfxCounter].clip = helloClip;
		IncreaseSFXCounter ();
	}
	public void PlayExcuseMeSFX()
	{
		sfxAudioSources [sfxCounter].clip = excuseMeClip;
		IncreaseSFXCounter ();
	}
	public void PlayClickSFX()
	{
		sfxAudioSources [sfxCounter].clip = clickClip;
		IncreaseSFXCounter ();
	}
	public void PlayEndOfLevelSFX()
	{
		sfxAudioSources [sfxCounter].clip = endOfLevelClip;
		IncreaseSFXCounter ();
	}
	public void PlayEnemyYawnSFX()
	{
		sfxAudioSources [sfxCounter].clip = enemyYawnClips[Random.Range(0,2)];
		IncreaseSFXCounter ();
	}
	public void PlayPlayerYawnSFX()
	{
		sfxAudioSources [sfxCounter].clip = playerYawnClips[Random.Range(0,2)];
		IncreaseSFXCounter ();
	}
	private void IncreaseSFXCounter()
	{
		sfxAudioSources [sfxCounter].Play ();
		sfxCounter ++;
		if (sfxCounter == sfxAudioSources.Count)
			sfxCounter = 0;
	}
}
