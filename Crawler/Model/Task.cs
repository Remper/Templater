using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crawler.Model
{
    enum Statuses { Open, Stopped, Inprogress, Started, Closed }

    /// <summary>
    /// Класс "Задача"
    /// </summary>
    public class Task
    {
        private int _ID;
        private int _TemplateID;
        private string _Website;
        private DateTime _Timestamp;
        private int _Depth;
        private Statuses _Status;
        private int _Results;
        private int _Progress;
        private int _curDepth;

        public Task(int id, int templateId, string website, string timestamp, int depth)
        {
            this._TemplateID = templateId;
            this._Website = website;
            this._Timestamp = DateTime.Parse(timestamp);
            this._Depth = depth;
            this._ID = id;
            _Status = Statuses.Open;
            _Progress = 0;
            _curDepth = 0;
            _Results = 0;
        }

        public void StartCrawling() 
        {

        }

        public void StopCrawling()
        {
           
        }

        private void FinalizeTask()
        {

        }

        //Readonly
        public int ID { get { return this._ID; } }
        public int TemplateID { get { return this._TemplateID; } }
        public string Website { get { return this._Website; } }
    }
}
