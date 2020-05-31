namespace DapperDemo.Model
{
    public class Data
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public override string ToString()
        {
            return $"Id: {Id}, Text: {Text}";
        }
    }
}