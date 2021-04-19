using Happify.Levels;
using Happify.TextToSpeech;
using Happify.User;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LevelsTTS : MonoBehaviour, IPointerClickHandler
{

    public AudioSource Audio;
	// Start is called before the first frame update

	private UserDescription currentUser;

	private float interval = 0.3f;
	int tap = 0;

	void Awake()
    {
		currentUser = UserManager.Instance.CurrentUser;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		string objName = eventData.selectedObject.name;

		if (!currentUser.RemainingVision && currentUser.RemainingHearing)
		{
			tap++;

			if (tap == 1)
			{
				StartCoroutine(DoubleTapInterval());
				OverarchingTTS.Instance.OnDoubleClick(objName, Audio);
			}
			else if (tap > 1)
			{
				OverarchingTTS.Instance.OnSingleClick(objName);
				tap = 0;
			}
		}
		else
			OverarchingTTS.Instance.OnSingleClick(objName);
	}

	IEnumerator DoubleTapInterval()
	{
		yield return new WaitForSeconds(interval);
		this.tap = 0;
	}

}
