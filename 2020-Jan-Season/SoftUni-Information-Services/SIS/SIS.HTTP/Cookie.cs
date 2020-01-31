namespace SIS.HTTP
{
    /// <summary>
    /// Represents an HTTP Cookie information, consisting of <c>Name</c> and <c>Value</c>.
    /// </summary>
    public class Cookie
    {
        //------------- CONSTRUCTORS -------------
        /// <summary>
        /// Initializes a new <see cref="Cookie"/> class.
        /// </summary>
        /// <param name="name">Cookie name</param>
        /// <param name="value">Cookie value</param>
        public Cookie(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        //-------------- PROPERTIES --------------
        /// <summary>
        /// HTTP Cookie Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// HTTP Cookie Value.
        /// </summary>
        public string Value { get; set; }
    }
}
