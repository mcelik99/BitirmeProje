using BitirmeProjesi.Models;

namespace BitirmeProjesi.Services
{
    public class ParticipantService
    {

        public Tuple<ParticipantTeacher?, bool> Display(int TeacherId, List<ParticipantTeacher> items)
        {

            foreach (var item in items)
            {
                if (item.Status == 1)
                {
                    return new Tuple<ParticipantTeacher, bool>(null, false);
                }

                if (item.Status == 0)
                {
                    if (item.TeacherId == TeacherId)
                    {
                        return new Tuple<ParticipantTeacher, bool>(item, true);
                    }

                    return new Tuple<ParticipantTeacher, bool>(null, false);
                }


            }
            return new Tuple<ParticipantTeacher, bool>(null, false);

        }

    }
}
