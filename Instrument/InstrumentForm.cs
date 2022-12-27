using System.Data;
using System.Drawing;
using System.Speech.Synthesis;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Instrument
{
    public partial class InstrumentForm : Form
    {
        private readonly IConfiguration _config;
        private readonly ILogger<InstrumentForm> _logger;
        private SpeechSynthesizer _speech = new() {Volume = 100};

        public InstrumentForm(IConfiguration config,ILogger<InstrumentForm> logger)
        {
            _config = config;
            _logger = logger;
            InitializeComponent();
        }

        


    }
}
