using MeasureService.Services.Models;
using Messages.Commands;
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
        private readonly IMessageSession _messageSession;

        public MeasureService(IMeasureRepository measureRepository, IMessageSession messageSession)
        {
            _measureRepository = measureRepository;
            _messageSession = messageSession;
        }

        public async Task Add(MeasureModel measure)
        {

          MeasureModel newMeasureModel=  await _measureRepository.Add(measure);

          await  _messageSession.Send<IUpdateUser>(message =>
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
            });
        }
    }
}
