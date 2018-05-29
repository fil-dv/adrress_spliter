using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Adr
{
    public partial class FormRes : Form
    {
        public FormRes()
        {
            InitializeComponent();
            SetData();
        }

        private void SetData()
        {
            try
            {
                label_total_yes.Text = (Mediator.ApYes + Mediator.AfYes + Mediator.AwYes + Mediator.AvrYes).ToString();
                label_total_no.Text = (Mediator.ApNo + Mediator.AfNo + Mediator.AwNo + Mediator.AvrNo).ToString();
                Loger.AddRecordToLog((Mediator.ApYes + Mediator.AfYes + Mediator.AwYes + Mediator.AvrYes).ToString() + " шт. успешно.");
                Loger.AddRecordToLog((Mediator.ApNo + Mediator.AfNo + Mediator.AwNo + Mediator.AvrNo).ToString() + " шт. не рассплитилось.");

                label_ap_yes.Text = Mediator.ApYes.ToString();
                label_ap_no.Text = Mediator.ApNo.ToString();

                label_af_yes.Text = Mediator.AfYes.ToString();
                label_af_no.Text = Mediator.AfNo.ToString();

                label_ar_yes.Text = Mediator.AwYes.ToString();
                label_ar_no.Text = Mediator.AwNo.ToString();

                label_avr_yes.Text = Mediator.AvrYes.ToString();
                label_avr_no.Text = Mediator.AvrNo.ToString();
            }
            catch (Exception ex)
            {
                Loger.AddRecordToLog(ex.Message);
            }
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
