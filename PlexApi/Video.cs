using System;
using System.Collections.Generic;
using RestSharp;

namespace PlexAPI
{
	public class Video : PlexItem
	{
		/*
		<Video 
		ratingKey="3790" 
		key="/library/metadata/3790" 
		studio="Touchstone Pictures" 
		type="movie" 
		title="10 Things I Hate About You" 
		contentRating="PG-13" 
		summary="10 Things I Hate About You is a 1999 American teen romantic comedy film. It is directed by Gil Junger and stars Julia Stiles and Heath Ledger. The screenplay was written by Karen McCullah Lutz and Kirsten Smith. The film, a modernization of Shakespeare's The Taming of the Shrew, is titled after a poem written by the film's female lead (played by Stiles) to describe her bittersweet romance with the male lead (played by Ledger). The film was released March 31, 1999, and it was a breakout success for stars Stiles and Ledger. The film marks the motion picture directing debut of Junger." 
		rating="7.4000000953674299" 
		viewCount="1" 
		year="1999" 
		tagline="How do I loathe thee? Let me count the ways." 
		thumb="/library/metadata/3790/thumb/1368592766" 
		art="/library/metadata/3790/art/1368592766" 
		duration="5856384" 
		originallyAvailableAt="1999-03-31" 
		addedAt="1368592671" 
		updatedAt="1368592766">
			<Media id="3590" duration="5856384" bitrate="9625" width="1920" height="1040" aspectRatio="1.85" audioChannels="6" audioCodec="dca" videoCodec="h264" videoResolution="1080" container="mkv" videoFrameRate="24p">
				<Part id="3593" key="/library/parts/3593/file.mkv" duration="5856384" file="/mnt/media/sync/shared_media/video/movies/10 Things I Hate About You [1999]/10 Things I Hate About You [1999].mkv" size="7046202179" container="mkv"/>
			</Media>
			<Genre tag="Coming of age"/>
			<Genre tag="Comedy"/>
			<Writer tag="Kirsten Smith"/>
			<Writer tag="Karen McCullah Lutz"/>
			<Director tag="Gil Junger"/>
			<Country tag="USA"/>
			<Role tag="Allison Janney"/>
			<Role tag="Larry Miller"/>
			<Role tag="Susan May Pratt"/>
		</Video>
		*/
		
		public int ratingKey { get; set; }
		public int parentRatingKey { get; set; }
		public int parentKey { get; set; }
		public string studio { get; set; }
		public string contentRating { get; set; }
		public string summary { get; set; }
		public float rating { get; set; }
		public int viewCount { get; set; }
		public int year { get; set; }
		public string tagline { get; set; }
		public long duration { get; set; }
		public string originallyAvailableAt { get; set; }
		public string addedAt { get; set; }
		public List<Media> media { get; set; }
		public List<Genre> genre { get; set; }
		public List<Writer> writer { get; set; }
		public List<Media> director { get; set; }
		public List<Country> country { get; set; }
		public List<Role> role { get; set; }

		public bool secondary { get; set; }
		public bool search { get; set; }
		public string prompt { get; set; }

		public Video () {}
		public Video (User user, Server server, String uri) : base(user, server, uri) {}

		public List<T> GetChildren<T>() where T : PlexItem
		{
			var request = new RestRequest();
			request.Resource = uri;

			var items = Execute<List<T>>(request, user);
			for (var i = 0; i < items.Count; i++) {
				items [i].user = user;
				items [i].server = server;
				if (items [i].key.StartsWith ("/")) {
					items [i].uri = items [i].key;
				} else {
					items [i].uri = uri + "/" + items [i].key;
				}

			}
			return items;
		}
	}
}

