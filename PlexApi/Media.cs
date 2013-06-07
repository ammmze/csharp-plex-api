using System;
using System.Collections.Generic;

namespace PlexAPI
{
	public class Media
	{
		/*
		<Media id="3590" duration="5856384" bitrate="9625" width="1920" height="1040" aspectRatio="1.85" audioChannels="6" audioCodec="dca" videoCodec="h264" videoResolution="1080" container="mkv" videoFrameRate="24p">
			<Part id="3593" key="/library/parts/3593/file.mkv" duration="5856384" file="/mnt/media/sync/shared_media/video/movies/10 Things I Hate About You [1999]/10 Things I Hate About You [1999].mkv" size="7046202179" container="mkv"/>
		</Media>
		*/
		public int id { get; set; }
		public long duration { get; set; }
		public int bitrate { get; set; }
		public int width { get; set; }
		public int height { get; set; }
		public float aspectRatio { get; set; }
		public int audioChannels { get; set; }
		public string audioCodec { get; set; }
		public string videoCodec { get; set; }
		public string videoResolution { get; set; }
		public string container { get; set; }
		public string videoFrameRate { get; set; }
		public List<Part> part { get; set; }

	}
}

