using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Item
{
    public class AddCommentResponse
    {
        public bool Succeeded { get; set; }
        public CommentDto? Comment { get; set; }
    }
}
