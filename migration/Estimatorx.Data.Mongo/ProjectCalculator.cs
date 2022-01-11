using System;
using System.Linq;

namespace Estimatorx.Data.Mongo
{
    public static class ProjectCalculator
    {
        /// <summary>
        /// Updates all the totals the specified <paramref name="project"/>.
        /// </summary>
        /// <param name="project">The project to total.</param>
        /// <returns></returns>
        public static Project UpdateTotals(Project project)
        {
            var hoursPerWeek = (double)project.HoursPerWeek;

            foreach (var section in project.Sections)
            {
                UpdateSection(project, section);

                // update parent totals
                project.TotalTasks += section.TotalTasks;
                project.TotalHours += section.TotalHours;
            }

            // calculate weeks after children total
            project.TotalWeeks = Math.Round(project.TotalHours / hoursPerWeek, 2);

            double contingencyPercent = project.ContingencyRate / 100;
            double contingencyHours = project.TotalHours * (1 + contingencyPercent);
            project.ContingencyHours = Convert.ToInt32(Math.Round(contingencyHours, 0));
            project.ContingencyWeeks = Math.Round(project.ContingencyHours / hoursPerWeek, 2);

            return project;
        }

        private static void UpdateSection(Project project, Section section)
        {
            foreach (var task in section.Tasks)
            {
                UpdateEstimate(project, task);

                // update parent totals
                section.TotalTasks += task.TotalTasks;
                section.TotalHours += task.TotalHours;
            }

            // calculate weeks after children total
            var hoursPerWeek = (double)project.HoursPerWeek;
            section.TotalWeeks = Math.Round(section.TotalHours / hoursPerWeek, 2);
        }

        private static void UpdateEstimate(Project project, Task task)
        {
            byte verySimple = task.VerySimple ?? 0;
            byte simple = task.Simple ?? 0;
            byte medium = task.Medium ?? 0;
            byte complex = task.Complex ?? 0;
            byte veryComplex = task.VeryComplex ?? 0;

            var factor = project.Factors.FirstOrDefault(f => f.Id == task.FactorId) ?? new Factor();

            task.TotalTasks = verySimple
                + simple
                + medium
                + complex
                + veryComplex;

            task.TotalHours = verySimple * factor.VerySimple
                + simple * factor.Simple
                + medium * factor.Medium
                + complex * factor.Complex
                + veryComplex * factor.VeryComplex;
        }
    }
}
