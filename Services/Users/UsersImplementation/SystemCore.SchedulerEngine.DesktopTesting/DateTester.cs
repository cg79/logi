using System;
using System.Windows.Forms;
using Scheduler.DataRequest;
using Scheduler.Logic;

using System.Collections.Generic;
using SchedulerEngine;
using SystemCore.SchedulerEngine.Request;
using ESB.Utils.Serializers;

namespace RecurrenceTester
{
    public partial class DateTester : Form
    {
        public DateTester()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            cmbFrequencyType.DataSource = Enum.GetValues(typeof(FrequenceType));
            cmbFrequencyRelative.DataSource = Enum.GetValues(typeof(FrequencyRelativeInterval));

            comboBox2.DataSource = Enum.GetValues(typeof(FrequencyRelativeInterval));
            comboBox3.DataSource = Enum.GetValues(typeof(FrequencyDaysValue));

            cboYearEveryMonth.DataSource = Enum.GetValues(typeof(FrequencyMonthValue));
              comboBox5.DataSource = Enum.GetValues(typeof(FrequencyRelativeInterval));
              comboBox4.DataSource = Enum.GetValues(typeof(FrequencyDaysValue));
              comboBox6.DataSource = Enum.GetValues(typeof(FrequencyMonthValue));


            
        }
        private void DateTester_Load(object sender, EventArgs e)
        {
            dtStartDate.Value = DateTime.Today;
            dtEndDate.Value = DateTime.Today.AddYears(10);

            // Monthly
            textBox4.Text = DateTime.Today.Day.ToString();
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;

            // Yearly
            cboYearEveryMonth.SelectedIndex = DateTime.Today.Month - 1;
            txtYearEvery.Text = DateTime.Today.Day.ToString();
            comboBox5.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;
            comboBox6.SelectedIndex = DateTime.Today.Month - 1;

            switch (DateTime.Today.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    chkSunday.Checked = true;
                    break;
                case DayOfWeek.Monday:
                    chkMonday.Checked = true;
                    break;
                case DayOfWeek.Tuesday:
                    chkTuesday.Checked = true;
                    break;
                case DayOfWeek.Wednesday:
                    chkWednesday.Checked = true;
                    break;
                case DayOfWeek.Thursday:
                    chkThursday.Checked = true;
                    break;
                case DayOfWeek.Friday:
                    chkFriday.Checked = true;
                    break;
                case DayOfWeek.Saturday:
                    chkSaturday.Checked = true;
                    break;
            }
        }

        private void GetRecurrence(DateTime startDate, DateTime? endDate)
        {
            SchedulerResponse values = null;


            switch (tabControl1.SelectedIndex)
            {
                case 0: // Daily

                    if (radioButton1.Checked)
                    {
                        values = SchedulerEngineManager.Daily().EveryXDays(int.Parse(textBox1.Text))

                        .SchedulerSettings(s => s.StartDate(startDate)
                                                 .EndDate(endDate)
                                                 .MaxOccurencesNo(Convert.ToInt32(txtOccurrences.Text))
                                                 .IntervalType((IntervalPattern)(int.Parse(pnlDateSettings.Tag.ToString()))))
                        .GetRecurrenceValues();
                    }
                    else
                    {
                        values = SchedulerEngineManager.Once()
                            .SchedulerSettings(s => s.StartDate(startDate)
                                                 .EndDate(endDate)
                                                     .MaxOccurencesNo(Convert.ToInt32(txtOccurrences.Text))
                                                     .IntervalType((IntervalPattern)(int.Parse(pnlDateSettings.Tag.ToString()))))
                            .GetRecurrenceValues();
                    }

                    break;

                case 1: // Weekly
                    //List<DayEnum> days = new List<DayEnum>();
                    int daysAsBitWise = 0;
                    if (chkSunday.Checked)
                    {
                        daysAsBitWise = daysAsBitWise + 1;
                    }
                    if (chkMonday.Checked)
                    {
                        daysAsBitWise = daysAsBitWise + 2;
                    }
                    if (chkTuesday.Checked)
                    {
                        daysAsBitWise = daysAsBitWise + 4;
                    }
                    if (chkWednesday.Checked)
                    {
                        daysAsBitWise = daysAsBitWise + 8;
                    }
                    if (chkThursday.Checked)
                    {
                        daysAsBitWise = daysAsBitWise + 16;
                    }
                    if (chkFriday.Checked)
                    {
                        daysAsBitWise = daysAsBitWise + 32;
                    }
                    if (chkSaturday.Checked)
                    {
                        daysAsBitWise = daysAsBitWise + 64;
                    }
                    values = SchedulerEngineManager.Weekly()
                         .SchedulerSettings(s => s.StartDate(startDate)
                                                 .EndDate(endDate)
                                                  .MaxOccurencesNo(Convert.ToInt32(txtOccurrences.Text))
                                                  .IntervalType((IntervalPattern)(int.Parse(pnlDateSettings.Tag.ToString()))))
                        .EveryXWeeks(int.Parse(txtWeeklyRegenXWeeks.Text))
                        //.SelectedDays(DayEnum.Monday, DayEnum.Sunday)
                         .SelectedDays(daysAsBitWise)
                        .GetRecurrenceValues();

                    break;

                case 2: // Monthly


                    if (radioSkipRecurrencePattern.Checked)
                    {
                        values = SchedulerEngineManager.Monthly()
                              .SchedulerSettings(s => s.StartDate(startDate)
                                                 .EndDate(endDate)
                                                 .MaxOccurencesNo(Convert.ToInt32(txtOccurrences.Text))
                                                 .IntervalType((IntervalPattern)(int.Parse(pnlDateSettings.Tag.ToString()))))
                               .EveryXMonths(int.Parse(textBox2.Text)).DayNo(Convert.ToInt32(textBox4.Text))
                               .GetRecurrenceValues();
                    }
                    else
                    {
                        values = SchedulerEngineManager.MonthlyRelative()
                             .SchedulerSettings(s => s.StartDate(startDate)
                                                 .EndDate(endDate)
                                                 .MaxOccurencesNo(Convert.ToInt32(txtOccurrences.Text))
                                                 .IntervalType((IntervalPattern)(int.Parse(pnlDateSettings.Tag.ToString()))))
                            .EveryXMonths(int.Parse(textBox3.Text))
                            .FrequencyRelative((FrequencyRelativeInterval)comboBox2.SelectedItem)
                            .FrequencyInterval((FrequencyDaysValue)comboBox3.SelectedItem)
                            .GetRecurrenceValues();

                    }


                    break;

                case 3: // Yearly

                    if (radioYearlyEvery.Checked)
                    {
                        values = SchedulerEngineManager.Yearly()
                           .SchedulerSettings(s => s.StartDate(startDate)
                                                 .EndDate(endDate)
                                                 .MaxOccurencesNo(Convert.ToInt32(txtOccurrences.Text))
                                                 .IntervalType((IntervalPattern)(int.Parse(pnlDateSettings.Tag.ToString()))))
                            .DayNo(int.Parse(txtYearEvery.Text)).Month((FrequencyMonthValue)(cboYearEveryMonth.SelectedItem))
                            .EveryXYears(Int32.Parse(txtYearlySkip.Text))
                            .GetRecurrenceValues();
                    }
                    else
                    {
                        values = SchedulerEngineManager.YearlyRelative()
                          .SchedulerSettings(s => s.StartDate(startDate)
                                                 .EndDate(endDate)
                                                .MaxOccurencesNo(Convert.ToInt32(txtOccurrences.Text))
                                                .IntervalType((IntervalPattern)(int.Parse(pnlDateSettings.Tag.ToString()))))

                           .FrequencyRelative((FrequencyRelativeInterval)comboBox5.SelectedItem)
                                              .FrequencyInterval((FrequencyDaysValue)comboBox4.SelectedItem)
                                              .FrequenceRecurrenceFactor((FrequencyMonthValue)(comboBox6.SelectedItem))
                                              .EveryXYears(Int32.Parse(txtYears.Text))
                                              .GetRecurrenceValues();
                    }


                    break;
            }

            ////

            ProcessResponse(values);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Foo.Builder.
            //SchedulerWrapperUtility.WithHealth(1).;
            //DateTime? endDate = 
             IntervalPattern intervalPat =   (IntervalPattern)(int.Parse(pnlDateSettings.Tag.ToString()));

            if(intervalPat == IntervalPattern.EndDate)
            {
                GetRecurrence(dtStartDate.Value,dtEndDate.Value);
            }else
            {
                GetRecurrence(dtStartDate.Value,null);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (txtNextEndDate.Text != "")
            {
                GetRecurrence(DateTime.Parse(txtNextGenDate.Text), DateTime.Parse(txtNextEndDate.Text));
            }
            else 
            {
                GetRecurrence(DateTime.Parse(txtNextGenDate.Text), null);
            }
            
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateItem dt = (DateItem)lstResults.SelectedItem;
            monthCalendar1.SetDate(dt.Value);

        }

      


        private void ProcessResponse(SchedulerResponse values)
        {
            if (values == null)
                return;
            //richTextBox1.Text = values.RecurrencePattern;
            lstResults.Items.Clear();
            DateTime[] bolded = new DateTime[values.Values.Count];
            int counter = 0;
            foreach (DateTime dt in values.Values)
            {
                bolded[counter] = dt;
                lstResults.Items.Add(new DateItem(dt));
                counter++;
            }
            monthCalendar1.BoldedDates = bolded;

            if (lstResults.Items.Count > 0)
                lstResults.SelectedIndex = 0;

            txtTotal.Text = lstResults.Items.Count.ToString();
            txtEndDate.Text = values.LastDate.ToShortDateString();
            txtStartDate.Text = values.StartDate.ToShortDateString();
            //txtNextGenDate.Text = values.NextStartDateRequest.ToShortDateString();
            //txtNextEndDate.Text = values.NextEndDateRequest.HasValue ? values.NextEndDateRequest.Value.ToShortDateString() : string.Empty;
           
            btnGetNextDate.Enabled = lstResults.Items.Count > 0;
            txtNextDate.Text = string.Empty;
            //lstRecurrenceValues.Items.Clear();
            tabMain.SelectedTab = tabSecond;
        }

       

        private void rdNotSet_Click(object sender, EventArgs e)
        {
            pnlDateSettings.Tag = rdNotSet.Tag;
        }

        private void radioOccurrences_Click(object sender, EventArgs e)
        {
            pnlDateSettings.Tag = radioOccurrences.Tag;
        }

        private void radioEndBy_Click(object sender, EventArgs e)
        {
            pnlDateSettings.Tag = radioEndBy.Tag;
        }

        private void radioButton1_Click(object sender, EventArgs e)
        {
            tabPage1.Tag = radioButton1.Tag;
        }

        private void radioButton2_Click(object sender, EventArgs e)
        {
            tabPage1.Tag = radioButton2.Tag;
        }

        private void btnGenerateByTable_Click(object sender, EventArgs e)
        {
            SchedulerIntervals intervals = new SchedulerIntervals();

            intervals.StartDate = dtStartDate.Value;

            intervals.MaxOccurencesNo = Convert.ToInt32(txtOccurrences.Text);
            intervals.EndDate = dtEndDate.Value;
            if (rdNotSet.Checked)
            {
                intervals.IntervalPattern = IntervalPattern.NotSet;
            }
            else
            {
                if (radioOccurrences.Checked)
                {
                    intervals.IntervalPattern = IntervalPattern.OccurenceNumber;
                }
                else
                {
                    intervals.IntervalPattern = IntervalPattern.EndDate;
                }
            }

            int frequencyInterval = 0;
            Int32.TryParse(txtFrequencyInterval.Text, out frequencyInterval);

            int frequencyRecurrenceFactor = 0;
            Int32.TryParse(txtFrequenceRecurrenceFactor.Text,out frequencyRecurrenceFactor);

            SchedulerJsonRequest schedulerJsonRequest = new SchedulerJsonRequest() 
            {
                interval = intervals,
                frequencyType = (FrequenceType)cmbFrequencyType.SelectedItem,
                frequencyInterval = frequencyInterval,
                frequencyRelativeInterval = (FrequencyRelativeInterval)cmbFrequencyRelative.SelectedItem,
                frequencyRecurrenceFactor = frequencyRecurrenceFactor
            };

            SchedulerResponse resp = SchedulerEngineManager.GetRecurrenceValuesFromJson(schedulerJsonRequest.ToJSON());

            //SchedulerResponse resp = SchedulerEngineManager.GetRecurrenceValues(intervals,
            //    (FrequenceType)cmbFrequencyType.SelectedItem,
            //    frequencyInterval,
            //    (FrequencyRelativeInterval)cmbFrequencyRelative.SelectedItem,
            //    frequencyRecurrenceFactor,
            //    Int32.Parse(txtYearsValue.Text));

            ProcessResponse(resp);
                
        }


    }
}