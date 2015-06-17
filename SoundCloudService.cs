using UnityEngine;
using System.Collections;

public class SoundCloudService : MonoBehaviour 
{
	public static SoundCloudService instance;
	public string clientId;
	protected SoundCloudTrackVO lastLoadedTrack;

	void Awake()
	{
		instance = this;
	}
	
	public SoundCloudTrackLoader createTrackLoader() {
		GameObject loaderObject = new GameObject("SoundCloudTrackLoader");
		SoundCloudTrackLoader theLoader = loaderObject.AddComponent<SoundCloudTrackLoader>();
		loaderObject.transform.parent = transform;

		theLoader.TrackLoaded += trackLoaded;
		theLoader.init(clientId);

		return theLoader;
	}

	public void LoadCommentUserAvatars(SoundCloudTrackVO aSoundCloudTrackVO) {
		foreach (SoundCloudCommentVO theComment in aSoundCloudTrackVO.comments) {
			ImageLoader theLoader = theComment.LoadAvatarImage();
			theLoader.imageLoaded += OnAvatarImageLoaded;
		}
	}

	void OnAvatarImageLoaded (ImageLoader aImageLoader)
	{
		aImageLoader.imageLoaded -= OnAvatarImageLoaded;
	}
	
	protected void trackLoaded(SoundCloudTrackLoader aLoader) {
		lastLoadedTrack = aLoader.soundCloudTrack;
		aLoader.TrackLoaded -= trackLoaded;
	}

}
