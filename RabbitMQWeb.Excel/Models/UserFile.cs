﻿using System.ComponentModel.DataAnnotations.Schema;

namespace RabbitMQWeb.Excel.Models
{
	public enum FileStatus
	{
		Creating = 0,
		Completed = 1
	}

	public class UserFile
	{
		public int Id { get; set; }
		public string? UserId { get; set; }
		public string? FileName { get; set; }
		public string? FilePath { get; set; }
		public DateTime? CreatedDate { get; set; }
		public FileStatus FileStatus { get; set; }

		[NotMapped]
		public string GetCreatedDate => CreatedDate.HasValue ? CreatedDate.Value.ToShortDateString() : "-";
    }
}
