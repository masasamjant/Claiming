using System.Text;

namespace Masasamjant.Claiming
{
    /// <summary>
    /// Represents view model that constain claim information.
    /// </summary>
    public class ClaimViewModel
    {
        /// <summary>
        /// Gets the claim descriptor.
        /// </summary>
        public ClaimDescriptor ClaimDescriptor { get; set; } = new ClaimDescriptor();

        /// <summary>
        /// Gets or sets the name of retry area.
        /// </summary>
        public string RetryAreaName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of retry controller.
        /// </summary>
        public string RetryControllerName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of retry action.
        /// </summary>
        public string RetryActionName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets CSS class of retry hyperlink.
        /// </summary>
        public string? RetryHyperlinkCssClass { get; set; }

        /// <summary>
        /// Gets the route parameters of retry hyperlink.
        /// </summary>
        public IDictionary<string, string> RetryRouteParameters { get; } = new Dictionary<string, string>();

        /// <summary>
        /// Gets whether or not retry hyperlink should be visible.
        /// </summary>
        public bool ShowRetryHyperlink => !string.IsNullOrWhiteSpace(RetryControllerName) && !string.IsNullOrWhiteSpace(RetryActionName);

        /// <summary>
        /// Gets HTML markup of retry hyperlink or empty string if not available or not visible.
        /// </summary>
        /// <param name="text">The text of the retry hyperlink.</param>
        /// <returns>A HTML markup of retry hyperlink or empty string.</returns>
        public string GetRetryHyperlink(string text)
        {
            if (!ClaimDescriptor.IsNotFound || !ShowRetryHyperlink)
                return string.Empty;

            var url = BuildRetryHyperlink();
            var css = RetryHyperlinkCssClass;

            if (string.IsNullOrWhiteSpace(css))
                return $"<a href=\"{url}\">{text}</a>";
            else
                return $"<a href=\"{url}\" class=\"{css}\">{text}</a>";
        }

        private string BuildRetryHyperlink()
        {
            var builder = new StringBuilder("/");

            var area = RetryAreaName;

            if (!string.IsNullOrWhiteSpace(area))
            {
                builder.Append(area);
                builder.Append('/');
            }

            builder.Append(RetryControllerName);
            builder.Append('/');
            builder.Append(RetryActionName);

            if (RetryRouteParameters.Count == 0)
                return builder.ToString();

            builder.Append('?');

            foreach (var keyValue in RetryRouteParameters)
            {
                builder.Append(keyValue.Key);
                builder.Append('=');
                builder.Append(keyValue.Value);
                builder.Append('&');
            }

            return builder.ToString().TrimEnd('&');
        }
    }
}
