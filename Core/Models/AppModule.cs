namespace Core.Models
{
    public class AppModule
    {
        public int ID { get; set; }
        public string? Title { get; set; }
        public string? Image {  get; set; }
        public string? Icon { get; set; }
        public string? Description { get; set; }
        public List<AppComponent>? Components { get; set; }
        public bool IsChosen { get; set; }
    }

}
