using UnityEngine;
using System.Collections;

public class InGameScriptRefrences : MonoBehaviour 
{
    //internal static GameUIController gameUIController;
    internal static GameVariables gameVariables;
    internal static PlayingObjectManager playingObjectManager;
    internal static PlayingObjectGeneration playingObjectGeneration;
    internal static StrikerManager strikerManager;
    internal static Striker striker;
    internal static ScoreManager scoreManager;
    internal static ParticleAudioManager bombParticleAudioManager;
    internal static ParticleAudioManager poppingParticleAudioManager;
    internal static ParticleAudioManager scoreParticleAudioManager;
    internal static SoundFxManager soundFxManager;
	internal static OnFireManager onFireManager;


    internal static Transform coinCollectionPosition;
    internal static Transform cutterCollectionPosition;
    internal static Transform bombCollectionPosition;

    void Awake()
    {
		onFireManager = GameObject.Find("panel_frame").GetComponent<OnFireManager>();
        //gameUIController = GameObject.Find("GUI Stuffs").GetComponent<GameUIController>();
        gameVariables = GameObject.Find("Game Variables").GetComponent<GameVariables>();
        playingObjectManager = GameObject.Find("Playing Object Manager").GetComponent<PlayingObjectManager>();
        playingObjectGeneration = GameObject.Find("Playing Object Generation").GetComponent<PlayingObjectGeneration>();
        strikerManager = GameObject.Find("Striker Manager").GetComponent<StrikerManager>();
        striker = GameObject.Find("Striker").GetComponent<Striker>();
        scoreManager = GameObject.Find("Score Manager").GetComponent<ScoreManager>();

        poppingParticleAudioManager = GameObject.Find("Popping Sounds").GetComponent<ParticleAudioManager>();
        scoreParticleAudioManager = GameObject.Find("Score Sounds").GetComponent<ParticleAudioManager>();

        soundFxManager = GameObject.Find("Sound Fx Manager").GetComponent<SoundFxManager>();

    }
}
