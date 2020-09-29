namespace SUS.HTTP
{
    public class Cookie
    {
        public Cookie(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        // _ga=GA1.2.198505690.1579630167
        public Cookie(string cookieAsString)
        {
            var cookieParts = cookieAsString.Split(new char[] { '=' }, 2);
            this.Name = cookieParts[0];
            this.Value = cookieParts[1];
        }

        public string Name { get; set; }

        public string Value { get; set; }

        public override string ToString()
        {
            return $"{this.Name}={this.Value}";
        }
    }
}