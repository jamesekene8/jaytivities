namespace API.DTOs
{
	public class FacebookDto
	{
        public string Id { get; set; }
		public string Email { get; set; }
		public string Name { get; set; }

		public FaceBookPicture Picture { get; set; }

    }

	public class FaceBookPicture
	{
		public FaceBookPictureData Data { get; set; }
	}


	public class FaceBookPictureData
	{
		public string Url { get; set; }
	}
}
