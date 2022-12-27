using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace Instrument;

public partial class LoginForm : Form
{
    private const int MaxLoginCount = 3;
    private readonly IConfiguration _config;
    private readonly ILogger<LoginForm> _logger;
    private readonly IServiceProvider _serviceProvider;

    public LoginForm(IConfiguration config,ILogger<LoginForm> logger, IServiceProvider serviceProvider)
    {
        _config = config;
        _logger = logger;
        _serviceProvider = serviceProvider;
        InitializeComponent();
    }

    private bool CheckSeriesNumber()
    {
        string? seriesNumber = _config["SeriesNumber"];
        if (string.IsNullOrEmpty(seriesNumber)) return false;
        return OnlineCheck(seriesNumber);
    }

    private bool OnlineCheck(string seriesNumber)
    {
        //TODO: 限于条件，不做具体实现
        return true;
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        txtSeriesNumber.Text = _config["SeriesNumber"];
        if (CheckSeriesNumber())
        {
            
            _logger.LogInformation("SeriesNumber is valid");
            DialogResult = DialogResult.OK;
            this.Hide();
            InstrumentForm instrumentform = _serviceProvider.GetRequiredService<InstrumentForm>();
            instrumentform.Show();
            
        }
        else
        {
            _logger.LogWarning("SeriesNumber is invalid");
            MessageBox.Show("序列号无效，请联系管理员");
        }
    }

    private void btnLogin_Click(object sender, EventArgs e)
    {
        int loginCount = string.IsNullOrEmpty(_config["LoginCount"]) ? 0 : int.Parse(_config["LoginCount"]);
        if (loginCount >= MaxLoginCount)
        {
            _logger.LogError("Login failed with too many times");
            MessageBox.Show("多次登录失败，我们的管理员会尽快联系您");
            Application.Exit();
        }
        else
        {
            if (!string.IsNullOrEmpty(txtSeriesNumber.Text))
            {
                _config["LoginCount"] = "0";
                _config["SeriesNumber"] = txtSeriesNumber.Text;
                _logger.LogInformation("SeriesNumber is valid");
                DialogResult = DialogResult.OK;
                this.Hide();
                InstrumentForm instrumentform = _serviceProvider.GetRequiredService<InstrumentForm>();
                instrumentform.Show();
                return;
            }
            _logger.LogWarning("Login failed with {loginCount} times", loginCount);
            MessageBox.Show("登录失败，请检查序列号");
            _config["LoginCount"] = (loginCount + 1).ToString();
        }

    }

    private void LoginTooManyTimesReport()
    {
        //TODO: 限于条件，不做具体实现
    }
}


public class LoginException : Exception
{
    
    public LoginException(string message) : base(message)
    {
    }
}