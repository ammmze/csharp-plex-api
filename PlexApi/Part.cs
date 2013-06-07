using System;

namespace PlexAPI
{
	public class Part
	{
		/*
		<Media id="3590" duration="5856384" bitrate="9625" width="1920" height="1040" aspectRatio="1.85" audioChannels="6" audioCodec="dca" videoCodec="h264" videoResolution="1080" container="mkv" videoFrameRate="24p">
			<Part id="3593" key="/library/parts/3593/file.mkv" duration="5856384" file="/mnt/media/sync/shared_media/video/movies/10 Things I Hate About You [1999]/10 Things I Hate About You [1999].mkv" size="7046202179" container="mkv"/>
		</Media>
		*/
		public int id { get; set; }
		public string key { get; set; }
		public long duration { get; set; }
		public string file { get; set; }
		public long size { get; set; }
		public string container { get; set; }
	}
}

