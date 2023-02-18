namespace RingFunctionAppHost.Models;

#pragma warning disable CS8618
#pragma warning disable IDE1006

public class OrchestratorTimelineResponse
{
    public Event[] items { get; set; }
    public SupplementalData supplemental_data { get; set; }
    public string pagination_key { get; set; }

    public class Event
    {
        public Cv cv { get; set; }
        public Device device { get; set; }
        public long duration_ms { get; set; }
        public string end_time { get; set; }
        public string error_message { get; set; }
        public string event_id { get; set; }
        public string event_type { get; set; }
        public bool had_subscription { get; set; }
        public bool is_favorite { get; set; }
        public string origin { get; set; }
        public string owner_id { get; set; }
        public Properties properties { get; set; }
        public string schema { get; set; }
        public string source_id { get; set; }
        public string source_type { get; set; }
        public string start_time { get; set; }
        public string state { get; set; }
        public string updated_at { get; set; }
        public Visualization visualizations { get; set; }
        public long session_duration { get; set; }
        public string recording_status { get; set; }

        public class Cv
        {
            public string detection_type { get; set; }
            public bool other_motion { get; set; }
            public bool? person_detected { get; set; }
            public bool? stream_broken { get; set; }
            public object cv_triggers { get; set; }
            public DetectionTypes[] detection_types { get; set; }

            public class DetectionTypes
            {
                public string detection_type { get; set; }
                public long[] verified_timestamps { get; set; }
            }
        }

        public class Device
        {
            public string description { get; set; }
            public long id { get; set; }
            public string type { get; set; }
        }

        public class Properties
        {
            public bool is_alexa { get; set; }
            public bool is_autoreply { get; set; }
            public bool is_sidewalk { get; set; }
            public string stark_resolution { get; set; }
            public bool stark_reviewed { get; set; }
        }

        public class Visualization
        {
            public CloudMedia cloud_media_visualization { get; set; }
            public Lite24x7 lite24x7_visualization { get; set; }
            public LocalMedia local_media_visualization { get; set; }
            public Map map_visualization { get; set; }
            public Radar radar_visualization { get; set; }
            public SingleCoordinate single_coordinate_visualization { get; set; }

            public class CloudMedia
            {
                public Media[] media { get; set; }
                public string schema { get; set; }

                public class Media
                {
                    public Metadata custom_metadata { get; set; }
                    public string file_family { get; set; }
                    public string file_type { get; set; }
                    public bool is_e2ee { get; set; }
                    public string manifest_id { get; set; }
                    public long playback_duration { get; set; }
                    public long preroll_duration_ms { get; set; }
                    public string url { get; set; }

                    public string schema { get; set; }
                    public string start_time { get; set; }
                    public string end_time { get; set; }
                    public string source { get; set; }

                    public class Metadata
                    {
                        public string clip_label { get; set; }
                        public int clip_order { get; set; }
                        public bool time_based_recording_enabled { get; set; }
                        public bool isBlackVideo { get; set; }
                        public bool is_cover_on { get; set; }

                        public string audio_encoding { get; set; }
                        public string video_encoding { get; set; }
                        public string source { get; set; }
                    }
                }
            }
            public class Lite24x7
            {
                public string schema { get; set; }
                public string[] media { get; set; }
            }

            public class LocalMedia
            {
                public string schema { get; set; }
                public Media[] media { get; set; }

                public class Media
                {
                    public Metadata custom_metadata { get; set; }
                    public string file_type { get; set; }
                    public long[] frames_timestamps { get; set; }
                    public bool is_e2ee { get; set; }
                    public string manifest_id { get; set; }
                    public long playback_duration { get; set; }
                    public long preroll_duration_ms { get; set; }
                    public string schema { get; set; }
                    public string url { get; set; }

                    public class Metadata
                    {
                        public string clip_label { get; set; }
                        public string clip_location { get; set; }
                        public int clip_order { get; set; }
                        public bool isBlackVideo { get; set; }
                        public bool is_cover_on { get; set; }
                    }
                }
            }
            public class Map
            {
                public int bearing { get; set; }
                public string id { get; set; }
                public double latitude { get; set; }
                public double longitude { get; set; }
                public string map_type { get; set; }
                public string schema { get; set; }
                public double zoomLevel { get; set; }
            }
            public class Radar
            {
                public object custom_metadata { get; set; }
                public string schema { get; set; }
                public string url { get; set; }
            }
            public class SingleCoordinate
            {
                public double lat { get; set; }
                public double lon { get; set; }
                public long pts { get; set; }
            }
        }
    }

    public class SupplementalData
    {
        public Footage[] footages { get; set; }
        public class Footage
        {
            public long duration_ms { get; set; }
            public string end_time { get; set; }
            public bool deleted { get; set; }
            public long[] snapshots { get; set; }
            public string source_id { get; set; }
            public string start_time { get; set; }
            public string storage_type { get; set; }
            public string url { get; set; }
            public string schema { get; set; }
        }
    }
}
#pragma warning restore CS8618
#pragma warning restore IDE1006
