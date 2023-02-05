﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit;
using CodeBeam.MudExtensions.UnitTests.Components;
using FluentAssertions;
using MudBlazor;
using MudExtensions;
using MudExtensions.Enums;

namespace CodeBeam.MudExtensions.UnitTest.Components
{
    [TestFixture]
    public class StepperTests : BunitTest
    {
        [Test]
        public void StepperRenderTest()
        {
            var comp = Context.RenderComponent<MudStepper>();
            comp.Instance.Steps.Count.Should().Be(0);
        }

        [Test]
        public async Task StepperPreventStepChangeDirectionIsForwardWhenCompletingStepOneOfOneTest()
        {
            // Arrange
            var lastStepChangeDirection = StepChangeDirection.None;
            var stepper = Context.RenderComponent<MudStepper>(
                ComponentParameterFactory.Parameter(
                    nameof(MudStepper.PreventStepChange),
                    new Func<StepChangeDirection, bool>((direction) =>
                    {
                        lastStepChangeDirection = direction;
                        return false;
                    })
                )
            );
            var step0 = Context.RenderComponent<MudStep>(
                ComponentParameterFactory.CascadingValue(stepper.Instance)
            );

            // Act
            await stepper.Instance.CompleteStep(0, moveToNextStep: true);

            // Assert
            lastStepChangeDirection.Should().Be(StepChangeDirection.Forward);
        }

        [Test]
        public async Task StepperPreventStepChangeDirectionIsForwardWhenSkippingStepOneOfOneTest()
        {
            // Arrange
            var lastStepChangeDirection = StepChangeDirection.None;
            var stepper = Context.RenderComponent<MudStepper>(
                ComponentParameterFactory.Parameter(
                    nameof(MudStepper.PreventStepChange),
                    new Func<StepChangeDirection, bool>((direction) =>
                    {
                        lastStepChangeDirection = direction;
                        return false;
                    })
                )
            );
            var step0 = Context.RenderComponent<MudStep>(
                ComponentParameterFactory.CascadingValue(stepper.Instance)
            );

            // Act
            await stepper.Instance.SkipStep(0, moveToNextStep: true);

            // Assert
            lastStepChangeDirection.Should().Be(StepChangeDirection.Forward);
        }

        [Test]
        public async Task StepperPreventStepChangeDirectionIsForwardWhenChangingFromStepOneToStepTwoTest()
        {
            // Arrange
            var lastStepChangeDirection = StepChangeDirection.None;
            var stepper = Context.RenderComponent<MudStepper>(
                ComponentParameterFactory.Parameter(
                    nameof(MudStepper.PreventStepChange),
                    new Func<StepChangeDirection, bool>((direction) =>
                    {
                        lastStepChangeDirection = direction;
                        return false;
                    })
                )
            );
            var step0 = Context.RenderComponent<MudStep>(
                ComponentParameterFactory.CascadingValue(stepper.Instance)
            );
            var step1 = Context.RenderComponent<MudStep>(
                ComponentParameterFactory.CascadingValue(stepper.Instance)
            );
            await stepper.Instance.SetActiveIndex(step0.Instance);

            // Act
            await stepper.Instance.SetActiveIndex(1); // go to next step

            // Assert
            lastStepChangeDirection.Should().Be(StepChangeDirection.Forward);
        }

        [Test]
        public async Task StepperPreventStepChangeDirectionIsBackwardWhenChangingFromStepTwoToStepOneTest()
        {
            // Arrange
            var lastStepChangeDirection = StepChangeDirection.None;
            var stepper = Context.RenderComponent<MudStepper>(
                ComponentParameterFactory.Parameter(
                    nameof(MudStepper.PreventStepChange),
                    new Func<StepChangeDirection, bool>((direction) =>
                    {
                        lastStepChangeDirection = direction;
                        return false;
                    })
                )
            );
            var step0 = Context.RenderComponent<MudStep>(
                ComponentParameterFactory.CascadingValue(stepper.Instance)
            );
            var step1 = Context.RenderComponent<MudStep>(
                ComponentParameterFactory.CascadingValue(stepper.Instance)
            );
            await stepper.Instance.SetActiveIndex(step1.Instance);

            // Act
            await stepper.Instance.SetActiveIndex(-1); // go to previous step

            // Assert
            lastStepChangeDirection.Should().Be(StepChangeDirection.Backward);
        }

        [Test]
        public async Task StepperPreventStepChangeDirectionIsNoneWhenChangingToTheSameStepTest()
        {
            // Arrange
            var lastStepChangeDirection = StepChangeDirection.None;
            var stepper = Context.RenderComponent<MudStepper>(
                ComponentParameterFactory.Parameter(
                    nameof(MudStepper.PreventStepChange),
                    new Func<StepChangeDirection, bool>((direction) =>
                    {
                        lastStepChangeDirection = direction;
                        return false;
                    })
                )
            );
            var step0 = Context.RenderComponent<MudStep>(
                ComponentParameterFactory.CascadingValue(stepper.Instance)
            );
            await stepper.Instance.SetActiveIndex(step0.Instance);

            // Act
            await stepper.Instance.SetActiveIndex(0); // go to same step

            // Assert
            lastStepChangeDirection.Should().Be(StepChangeDirection.None);
        }

        [Test]
        public async Task StepperPreventStepChangeIsInvokedWhenCompletingActiveStepTest()
        {
            // Arrange
            var preventStepChangeWasInvoked = false;
            var stepper = Context.RenderComponent<MudStepper>(
                ComponentParameterFactory.Parameter(
                    nameof(MudStepper.PreventStepChange),
                    new Func<StepChangeDirection, bool>((direction) =>
                    {
                        preventStepChangeWasInvoked = true;
                        return false;
                    })
                )
            );
            var step0 = Context.RenderComponent<MudStep>(
                ComponentParameterFactory.CascadingValue(stepper.Instance)
            );
            var step1 = Context.RenderComponent<MudStep>(
                ComponentParameterFactory.CascadingValue(stepper.Instance)
            );
            await stepper.Instance.SetActiveIndex(step0.Instance);

            // Act
            await stepper.Instance.CompleteStep(stepper.Instance.Steps.IndexOf(step0.Instance));

            // Assert
            preventStepChangeWasInvoked.Should().Be(true);
        }

        [Test]
        public async Task StepperPreventStepChangeIsInvokedWhenSkippingActiveStepTest()
        {
            // Arrange
            var preventStepChangeWasInvoked = false;
            var stepper = Context.RenderComponent<MudStepper>(
                ComponentParameterFactory.Parameter(
                    nameof(MudStepper.PreventStepChange),
                    new Func<StepChangeDirection, bool>((direction) =>
                    {
                        preventStepChangeWasInvoked = true;
                        return false;
                    })
                )
            );
            var step0 = Context.RenderComponent<MudStep>(
                ComponentParameterFactory.CascadingValue(stepper.Instance)
            );
            var step1 = Context.RenderComponent<MudStep>(
                ComponentParameterFactory.CascadingValue(stepper.Instance)
            );
            await stepper.Instance.SetActiveIndex(step0.Instance);

            // Act
            await stepper.Instance.SkipStep(stepper.Instance.Steps.IndexOf(step0.Instance));

            // Assert
            preventStepChangeWasInvoked.Should().Be(true);
        }

        [Test]
        public async Task StepperPreventStepChangeIsNotInvokedWhenCompletingNonActiveStepTest()
        {
            // Arrange
            var preventStepChangeWasInvoked = false;
            var stepper = Context.RenderComponent<MudStepper>(
                ComponentParameterFactory.Parameter(
                    nameof(MudStepper.PreventStepChange),
                    new Func<StepChangeDirection, bool>((direction) =>
                    {
                        preventStepChangeWasInvoked = true;
                        return false;
                    })
                )
            );
            var step0 = Context.RenderComponent<MudStep>(
                ComponentParameterFactory.CascadingValue(stepper.Instance)
            );
            var step1 = Context.RenderComponent<MudStep>(
                ComponentParameterFactory.CascadingValue(stepper.Instance)
            );
            await stepper.Instance.SetActiveIndex(step0.Instance);

            // Act
            await stepper.Instance.CompleteStep(stepper.Instance.Steps.IndexOf(step1.Instance));

            // Assert
            preventStepChangeWasInvoked.Should().Be(false);
        }

        [Test]
        public async Task StepperPreventStepChangeIsNotInvokedWhenSkippingNonActiveStepTest()
        {
            // Arrange
            var preventStepChangeWasInvoked = false;
            var stepper = Context.RenderComponent<MudStepper>(
                ComponentParameterFactory.Parameter(
                    nameof(MudStepper.PreventStepChange),
                    new Func<StepChangeDirection, bool>((direction) =>
                    {
                        preventStepChangeWasInvoked = true;
                        return false;
                    })
                )
            );
            var step0 = Context.RenderComponent<MudStep>(
                ComponentParameterFactory.CascadingValue(stepper.Instance)
            );
            var step1 = Context.RenderComponent<MudStep>(
                ComponentParameterFactory.CascadingValue(stepper.Instance)
            );
            await stepper.Instance.SetActiveIndex(step0.Instance);

            // Act
            await stepper.Instance.SkipStep(stepper.Instance.Steps.IndexOf(step1.Instance));

            // Assert
            preventStepChangeWasInvoked.Should().Be(false);
        }

        [Test]
        public async Task StepperActiveIndexIsNotChangedWhenCompletingNonActiveStepTest()
        {
            // Arrange
            var stepper = Context.RenderComponent<MudStepper>();
            var step0 = Context.RenderComponent<MudStep>(
                ComponentParameterFactory.CascadingValue(stepper.Instance)
            );
            var step1 = Context.RenderComponent<MudStep>(
                ComponentParameterFactory.CascadingValue(stepper.Instance)
            );
            await stepper.Instance.SetActiveIndex(stepper.Instance.Steps.IndexOf(step0.Instance));

            // Act
            await stepper.Instance.CompleteStep(stepper.Instance.Steps.IndexOf(step1.Instance), moveToNextStep: true);

            // Assert
            stepper.Instance.ActiveIndex.Should().Be(stepper.Instance.Steps.IndexOf(step0.Instance));
        }

        [Test]
        public async Task StepperActiveIndexIsNotChangedWhenSkippingNonActiveStepTest()
        {
            // Arrange
            var stepper = Context.RenderComponent<MudStepper>();
            var step0 = Context.RenderComponent<MudStep>(
                ComponentParameterFactory.CascadingValue(stepper.Instance)
            );
            var step1 = Context.RenderComponent<MudStep>(
                ComponentParameterFactory.CascadingValue(stepper.Instance)
            );
            await stepper.Instance.SetActiveIndex(stepper.Instance.Steps.IndexOf(step0.Instance));

            // Act
            await stepper.Instance.SkipStep(stepper.Instance.Steps.IndexOf(step1.Instance), moveToNextStep: true);

            // Assert
            stepper.Instance.ActiveIndex.Should().Be(stepper.Instance.Steps.IndexOf(step0.Instance));
        }
    }
}