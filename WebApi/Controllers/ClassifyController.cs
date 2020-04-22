using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using LAB2;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Data.Request;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("api/classify")]
    public class ClassifyController
    {
        public ClassifyController()
        {
        }

        [HttpPost("GetTables")]
        public List<ClassificationTable2> GetTables([FromBody] ClassifyRequest requestBody)
        {
            var criteria = Common.Extensions.ToCriterion(requestBody.Criteria);
            var answers = requestBody.Answers;
            var tables = new List<ClassificationTable2>();
            var table = new ClassificationTable2(criteria);
            {
                int i = 0;
                while (!table.isClassified())
                {
                    if (i >= answers.Count) throw new Exception("Table can't be classified. No more answers");
                    table.NextStep(answers[i]);
                    table.Iteration = i;
                    var serialized = JsonSerializer.Serialize(table);
                    var deserialized = JsonSerializer.Deserialize<ClassificationTable2>(serialized);
                    tables.Add(deserialized);
                    ++i;
                }
            }
            return tables;
        }
    }
}