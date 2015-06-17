using UnityEngine;
using SimpleJSON;
using System.Collections;
using System.IO;

public delegate void SoundCloudTrackLoaderDelegate(SoundCloudTrackLoader aSoundCloudTrackLoader);

public class SoundCloudTrackLoader : MonoBehaviour {
	
	public const string API = "http://api.soundcloud.com";

	public SoundCloudTrackVO soundCloudTrack;
	
	protected string _trackId;
	protected string _errorMessage;
	protected string _trackInfoText;
	protected string _trackCommentsText;
	protected bool _loadErrorTriggered = false; 

	public string TrackId {
		get {
			return _trackId;
		}
	}

	public string FallbackTrackInfoFilePath {
		get {
			return  Application.streamingAssetsPath + "/" + _trackId + "_track.json";
		}
	}

	public string FallbackTrackCommentFilePath {
		get {
			return  Application.streamingAssetsPath + "/" + _trackId + "_comments.json";
		}
	}

	public string TrackInfoFilePath {
		get {
			return Application.persistentDataPath + "/" + _trackId + "_track.json";
		}
	}

	public string TrackCommentFilePath {
		get {
			return Application.persistentDataPath + "/" + _trackId + "_comments.json";
		}
	}

	public string TrackCommentsText {
		get {
			return _trackCommentsText;
		}
	}

	public string TrackInfoText {
		get {
			return _trackInfoText;
		}
	}

	public string ErrorMessage {
		get {
			return _errorMessage;
		}
	}

	protected string _clientId;
	protected bool _loadUserAvatars;

	protected JSONNode trackJSON;
	protected JSONNode commentsJSON;
	
	public event SoundCloudTrackLoaderDelegate LoadingError;
	protected event SoundCloudTrackLoaderDelegate _trackLoaded;
	
	public event SoundCloudTrackLoaderDelegate TrackLoaded
	{
		add
		{
			_trackLoaded += value;
		}
		remove
		{
			_trackLoaded -= value;

			//nobody is listening to the loader anymore, lets delete it
			if (_trackLoaded == null) {
				GameObject.Destroy(gameObject);
			}
		}
	}

	public void init(string aClientId) {
		_clientId = aClientId;
	}

	public void load(string aTrackId, bool aLoadUserAvatars = false) {
		_loadErrorTriggered = false;
		_trackId = aTrackId;
		_loadUserAvatars = aLoadUserAvatars;
		StartCoroutine(loadTrackCoRoutine(_trackId, _clientId));
		StartCoroutine(loadCommentsCoRoutine(_trackId, _clientId));
	}

	IEnumerator loadTrackCoRoutine(string aTrackId, string aClientId) {
		
		string theClient = "client_id=" + aClientId;
		string aQuery = aTrackId + ".json?" + theClient;
		string theUrl = string.Join("/", new string[3]{API, "tracks", aQuery});

		WWW loader = new WWW(theUrl);
		yield return loader;
	
		if (loader.error == null) {
			trackJSON = JSON.Parse(loader.text);
			_trackInfoText = loader.text;
			CheckAllLoaded();
		} else {
			//fallback: load last local version of the file:
			if (File.Exists(TrackInfoFilePath)) {
				StartCoroutine(loadLocalTrackFile(TrackInfoFilePath));
			} else if (File.Exists(FallbackTrackInfoFilePath)) { //fallback 2: load base file. 
				StartCoroutine(loadLocalTrackFile(FallbackTrackInfoFilePath));
			} else{
				ReportLoadingError(loader);
			}
		
		}
	}

	IEnumerator loadLocalTrackFile(string aPath) {
		WWW loader = new WWW("file://" + aPath);
		yield return loader;

		if (loader.error == null) {
			trackJSON = JSON.Parse(loader.text);
			_trackInfoText = loader.text;
			CheckAllLoaded();
		} else {
			ReportLoadingError(loader);
		}
	}
	

	IEnumerator loadCommentsCoRoutine(string aTrackId, string aClientId) {
		
		string theClient = "client_id=" + aClientId;
		string aQuery = "comments.json?" + theClient;
		string theUrl = string.Join("/", new string[4]{API, "tracks", aTrackId, aQuery});

		WWW loader = new WWW(theUrl);
		yield return loader;

		if (loader.error == null) {
			commentsJSON = JSON.Parse(loader.text);
			_trackCommentsText = loader.text;
			CheckAllLoaded();
		} else {
			//fallback: load last local version of the file:
			if (File.Exists(TrackCommentFilePath)) {
				StartCoroutine(loadLocalCommentsFile(TrackCommentFilePath));
			} else if (File.Exists(FallbackTrackCommentFilePath)) { //fallback 2: load base file. 
				StartCoroutine(loadLocalCommentsFile(FallbackTrackCommentFilePath));
			} else{
				ReportLoadingError(loader);
			}
		}
	}

	IEnumerator loadLocalCommentsFile(string aPath) {
		WWW loader = new WWW("file://" + aPath);
		yield return loader;
	
		if (loader.error == null) {
			commentsJSON = JSON.Parse(loader.text);
			_trackCommentsText = loader.text;
			CheckAllLoaded();
		} else {
			ReportLoadingError(loader);
		}
	}

	protected void CheckAllLoaded() {
		if (trackJSON != null && commentsJSON != null) {
			createTrackVO(trackJSON, commentsJSON);
		}
	}

	protected void ReportLoadingError(WWW aLoader) {
		if (_loadErrorTriggered)
			return;

		_errorMessage = aLoader.error;
		_loadErrorTriggered = true;
		
		if (LoadingError != null) {
			LoadingError.Invoke(this);
		}
	}

	protected void createTrackVO(JSONNode aTrackJSON, JSONNode aCommentsJSON) {
		soundCloudTrack = SoundCloudTrackVO.CreateFromJSON(aTrackJSON, aCommentsJSON);

		if (_trackLoaded != null)
			_trackLoaded(this);

		aTrackJSON = null;
		aCommentsJSON = null;
	}
}
