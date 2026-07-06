using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace MicroserviesWebApplication.Data
{
    public class SqlParamData
    {
        public SqlParamData(string _strName, object _objValue, SqlDbType _dtDataType, int _lLength, ParameterDirection _pdDirection)
        {
            this._strName = _strName;
            this._objValue = _objValue;
            this._dtDataType = _dtDataType;
            this._lLength = _lLength;
            this._pdDirection = _pdDirection;
        }

        private string _strName = string.Empty;
        //private string _strValue = string.Empty;
        private object _objValue = null;
        private SqlDbType _dtDataType;
        private int _lLength = 0;
        private ParameterDirection _pdDirection;

        public string Name
        {
            get { return _strName; }
            set { _strName = value; }
        }
        //public string Value
        //{
        //    get { return _strValue; }
        //    set { _strValue = value; }
        //}
        public object Value
        {
            get { return _objValue; }
            set { _objValue = value; }
        }
        public SqlDbType DataType
        {
            get { return _dtDataType; }
            set { _dtDataType = value; }
        }
        public int Length
        {
            get { return _lLength; }
            set { _lLength = value; }
        }
        public ParameterDirection Direction
        {
            get { return _pdDirection; }
            set { _pdDirection = value; }
        }
    }
}
