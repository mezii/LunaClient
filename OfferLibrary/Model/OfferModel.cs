using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfferLibrary.Model
{
    public class OfferModel
    {
        public int OfferId { get; set; }
        public string DeviceId { get; set; }
        public string Name { get; set; }

        public bool IsActive { get; set; }
        public int CTITAppStore { get; set; }
        public bool IsRandomCTIT { get; set; }

        public int FromRandomCTIT { get; set; }
        public int ToRandomCTIT { get; set; }
        public string  TrackingLink { get; set; }
        public string AppName { get; set; }

        public int InAppCTIT { get; set; }
        public string ScriptName { get; set; }

        public bool IsRandomScript { get; set; }
        
    }
}
