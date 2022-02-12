using System;
using System.Collections.Generic;

namespace Report.API.Application.Contracts.Dtos.ReviewDtos
{
    public class ReviewCreateDto
    {
        public int ExamId { get; private set; }
        public string ApplicantId { get; private set; }

        public ReviewCreateDto()
        {
        }

        public ReviewCreateDto(int examId, string applicantId)
        {
            ExamId = examId;
            ApplicantId = applicantId;
        }
    }
}