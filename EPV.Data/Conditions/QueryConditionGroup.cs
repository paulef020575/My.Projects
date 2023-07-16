﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;

namespace EPV.Data.Conditions
{
    public class QueryConditionGroup : List<ICondition>, ICondition
    {
        private LogicOperation _logicOperation;
        private bool _isNumerated;
        public QueryConditionGroup(LogicOperation logicOperation = LogicOperation.AND)
        {
            _logicOperation = logicOperation;
            _isNumerated = false;
        }

        public override string ToString()
        {
            if (!_isNumerated) SetNum();
            string separator = ") AND (";
            if (_logicOperation == LogicOperation.OR) separator = ") OR (";
            string expression = string.Join(separator, this.Select(x => x.ToString()));

            return $"({expression})";
        }

        public Dictionary<string, object> GetParameters()
        {
            if (!_isNumerated) SetNum();

            Dictionary<string, object> result = new Dictionary<string, object>();
            foreach (ICondition condition in this)
            {
                Dictionary<string, object> part = condition.GetParameters();
                foreach (string key in part.Keys)
                {
                    result.Add(key, part[key]);
                }
            }

            return result;
        }

        public int SetNum(int startNum = 0)
        {
            int result = startNum;
            foreach (ICondition condition in this)
                result = condition.SetNum(result);

            _isNumerated = true;
            return result;
        }
    }
}
