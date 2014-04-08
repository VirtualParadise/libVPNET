namespace VP
{
    /// <summary>
    /// List of target containers that a URL can be opened in
    /// </summary>
    public enum UrlTarget : int
    {
        /// <summary>
        /// Requests that the URL be opened in an external browser
        /// </summary>
        Browser,
        /// <summary>
        /// Requests that the URL be opened on top of the 3D viewport
        /// </summary>
        Overlay
    }
}
