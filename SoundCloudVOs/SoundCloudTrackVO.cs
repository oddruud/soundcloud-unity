using UnityEngine;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;


public class SoundCloudTrackVO {

	public string id;
	public string title;
	public int duration;
	public List<SoundCloudCommentVO> comments;
	
	public SoundCloudTrackVO() {}

	public static SoundCloudTrackVO CreateFromJSON(JSONNode aTrackJSON, JSONNode aCommentsJSON) {
		SoundCloudTrackVO soundCloudTrack = new SoundCloudTrackVO();

		soundCloudTrack.id =  aTrackJSON["id"].Value;
		soundCloudTrack.title = aTrackJSON["title"].Value; 
		soundCloudTrack.duration =  aTrackJSON["duration"].AsInt; 

		JSONArray theComments = aCommentsJSON.AsArray;
		soundCloudTrack.comments= new List<SoundCloudCommentVO>();
		foreach(JSONNode aComment in theComments.Childs) {
			SoundCloudCommentVO theComment = SoundCloudCommentVO.CreateFromJSON(aComment);
			soundCloudTrack.comments.Add(theComment);
		}

		return soundCloudTrack;
	}
}
