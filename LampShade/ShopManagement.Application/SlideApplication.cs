﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Application;
using ShopManagement.Application.Contracts.Slide;
using ShopManagement.Domain.SlideAgg;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ShopManagement.Application
{
    public class SlideApplication:ISlideApplication
    {
        private readonly ISlideRepository _SlideRepository;

        public SlideApplication(ISlideRepository slideRepository)
        {
            _SlideRepository = slideRepository;
        }
        public OperationResult Create(CreateSlide command)
        {
            var operation=new OperationResult();
            var slide = new Slide(command.Picture, command.PictureAlt, command.PictureTitle, command.Heading,
                command.Title, command.Text, command.Link,command.BtnText);
                _SlideRepository.Create(slide);
                _SlideRepository.SaveChanges();
                return operation.Succedded();
        }

        public OperationResult Edit(EditSlide command)
        {
            var operation = new OperationResult();

            var slide = _SlideRepository.Get(command.Id);
            if (slide == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            slide.Edit(command.Picture,command.PictureAlt,command.PictureTitle,command.Heading,command.Title,command.Text,command.Link,command.BtnText);
            _SlideRepository.SaveChanges();
            return operation.Succedded();
        }

        public OperationResult Remove(long id)
        {
            var operation = new OperationResult();

            var slide = _SlideRepository.Get(id);

            if (slide == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            slide.Remove();
            _SlideRepository.SaveChanges();
            return operation.Succedded();
        }
        public OperationResult Restore(long id)
        {
            var operation = new OperationResult();

            var slide = _SlideRepository.Get(id);

            if (slide == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            slide.Restore();
            _SlideRepository.SaveChanges();
            return operation.Succedded();
        }
        public EditSlide GetDetails(long id)
        {
            return _SlideRepository.GetDetails(id);
        }

        public List<SlideViewModel> GetList()
        {
            return _SlideRepository.GetList();
        }
    }
}
