using MeasureService.Services.Models;
using Messages.Commands;
using Messages.Enums.MeasureStatus;
using Messages.Events;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MeasureService.Services
{
    public class MeasureService : IMeasureService
    {
        private readonly IMeasureRepository _measureRepository;

        public MeasureService(IMeasureRepository measureRepository)
        {
            _measureRepository = measureRepository;
        }

        public async Task<MeasureModel> Add(MeasureModel measure)
        {

            measure.Status = MeasureStatus.Pending;
            MeasureModel newMeasureModel = await _measureRepository.Add(measure);

            return newMeasureModel;

           /* await _messageSession.Send<IUpdateUserFile>(message =>
              {
                  message.MeasureId = newMeasureModel.Id;
                  message.Weight = measure.Weight;
                  message.UserFileId = measure.UserFileId;
              });*/




            /* await  _messageSession.Send<IUpdateUser>(message =>
               {
                   message.MeasureId = measure.Id;
                   message.Weight = measure.Weight;
                   message.UserFileId = measure.UserFileId;
               });

               await _messageSession.Publish<IMeasureUpdated>(message =>
               {
                   message.MeasureId = newMeasureModel.Id;
                   message.UserFileId = newMeasureModel.UserFileId;
                   message.Weight = newMeasureModel.Weight;
               });*/
        }

        public async Task UpdateStatus(Guid measureId, MeasureStatus status, string comments)
        {
            await _measureRepository.UpdateStatusAsync(measureId, status, comments);

        }
    }
}
