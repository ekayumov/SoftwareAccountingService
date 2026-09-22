using System;
using System.Collections.Generic;
using System.Text;

namespace SoftwareAccountingService.Wpf.Presentation.Models
{
    public sealed class InspectionObjectModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Version { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public string Result { get; set; } = string.Empty;

        public DateTime ReceivedDate { get; set; }

        public string? Note { get; set; }
    }
}
