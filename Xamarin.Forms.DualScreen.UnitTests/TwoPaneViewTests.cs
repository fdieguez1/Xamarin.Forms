﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.DualScreen.UnitTests
{
	[TestFixture]
    public class TwoPaneViewTests : BaseTestFixture
	{
		[SetUp]
		public override void Setup()
		{
			base.Setup();
			Device.PlatformServices = new MockPlatformServices();
			Device.info = new TestDeviceInfo();
		}

		TwoPaneView CreateTwoPaneView(View pane1 = null, View pane2 = null)
		{
			TwoPaneView view = new TwoPaneView()
			{
				IsPlatformEnabled = true,
				Pane1 = pane1,
				Pane2 = pane2
			};

			if (pane1 != null)
				pane1.IsPlatformEnabled = true;

			if (pane2 != null)
				pane2.IsPlatformEnabled = true;

			view.Children[0].IsPlatformEnabled = true;
			view.Children[1].IsPlatformEnabled = true;

			return view;
		}

		[Test]
		public void GettersAndSetters()
		{
			var Pane1 = new StackLayout();
			var	Pane2 = new Grid();

			TwoPaneView twoPaneView = new TwoPaneView()
			{
				TallModeConfiguration = TwoPaneViewTallModeConfiguration.SinglePane,
				WideModeConfiguration = TwoPaneViewWideModeConfiguration.SinglePane,
				Pane1 = Pane1,
				Pane2 = Pane2,
				PanePriority = TwoPaneViewPriority.Pane2,
				MinTallModeHeight = 1000,
				MinWideModeWidth = 2000,
			};

			Assert.AreEqual(TwoPaneViewTallModeConfiguration.SinglePane, twoPaneView.TallModeConfiguration);
			Assert.AreEqual(TwoPaneViewWideModeConfiguration.SinglePane, twoPaneView.WideModeConfiguration);
			Assert.AreEqual(Pane1, twoPaneView.Pane1);
			Assert.AreEqual(Pane2, twoPaneView.Pane2);
			Assert.AreEqual(TwoPaneViewPriority.Pane2, twoPaneView.PanePriority);
			Assert.AreEqual(1000, twoPaneView.MinTallModeHeight);
			Assert.AreEqual(2000, twoPaneView.MinWideModeWidth);
		}

		[Test]
		public void BasicLayoutTest()
		{
			TwoPaneView twoPaneView = new TwoPaneView();
			twoPaneView.Layout(new Rectangle(0, 0, 300, 300));

			Assert.AreEqual(300, twoPaneView.Height);
			Assert.AreEqual(300, twoPaneView.Width);
		}

		[Test]
		public void ModeSwitchesWithMinWideModeWidth()
		{
			TwoPaneView twoPaneView = new TwoPaneView();
			twoPaneView.Layout(new Rectangle(0, 0, 300, 300));

			twoPaneView.MinWideModeWidth = 400;
			Assert.AreEqual(TwoPaneViewMode.SinglePane, twoPaneView.Mode);
			twoPaneView.MinWideModeWidth = 100;
			Assert.AreEqual(TwoPaneViewMode.Wide, twoPaneView.Mode);
		}

		[Test]
		public void ModeSwitchesWithMinTallModeHeight()
		{
			TwoPaneView twoPaneView = new TwoPaneView();
			twoPaneView.Layout(new Rectangle(0, 0, 300, 300));

			twoPaneView.MinTallModeHeight = 400;
			Assert.AreEqual(TwoPaneViewMode.SinglePane, twoPaneView.Mode);
			twoPaneView.MinTallModeHeight = 100;
			Assert.AreEqual(TwoPaneViewMode.Tall, twoPaneView.Mode);
		}

		[Test]
		public void Pane1LengthTallMode()
		{
			var pane1 = new BoxView();
			var pane2 = new BoxView();

			TwoPaneView twoPaneView = CreateTwoPaneView(pane1, pane2);

			twoPaneView.Layout(new Rectangle(0, 0, 300, 300));
			twoPaneView.MinTallModeHeight = 100;
			Assert.AreNotEqual(100, twoPaneView.Pane1.Height);
			twoPaneView.Pane1Length = 100;
			Assert.AreEqual(100, twoPaneView.Pane1.Height);
		}


		[Test]
		public void Pane1LengthWideMode()
		{
			var pane1 = new BoxView();
			var pane2 = new BoxView();

			TwoPaneView twoPaneView = CreateTwoPaneView(pane1, pane2);

			twoPaneView.Layout(new Rectangle(0, 0, 300, 300));
			twoPaneView.MinWideModeWidth = 100;
			Assert.AreNotEqual(100, twoPaneView.Pane1.Width);
			twoPaneView.Pane1Length = 100;
			Assert.AreEqual(100, twoPaneView.Pane1.Width);
		}


		[Test]
		public void Pane2LengthTallMode()
		{
			var pane1 = new BoxView();
			var pane2 = new BoxView();

			TwoPaneView twoPaneView = CreateTwoPaneView(pane1, pane2);

			twoPaneView.Layout(new Rectangle(0, 0, 300, 300));
			twoPaneView.MinTallModeHeight = 100;
			Assert.AreNotEqual(100, twoPaneView.Pane2.Height);
			twoPaneView.Pane2Length = 100;
			Assert.AreEqual(100, twoPaneView.Pane2.Height);
		}


		[Test]
		public void Pane2LengthWideMode()
		{
			var pane1 = new BoxView();
			var pane2 = new BoxView();

			TwoPaneView twoPaneView = CreateTwoPaneView(pane1, pane2);

			twoPaneView.Layout(new Rectangle(0, 0, 300, 300));
			twoPaneView.MinWideModeWidth = 100;
			Assert.AreNotEqual(100, twoPaneView.Pane2.Width);
			twoPaneView.Pane2Length = 100;
			Assert.AreEqual(100, twoPaneView.Pane2.Width);
		}


		[Test]
		public void PanePriority()
		{
			var pane1 = new BoxView();
			var pane2 = new BoxView();

			TwoPaneView twoPaneView = CreateTwoPaneView(pane1, pane2);

			twoPaneView.Layout(new Rectangle(0, 0, 300, 300));
			twoPaneView.MinWideModeWidth = 500;
			twoPaneView.MinTallModeHeight = 500;

			Assert.LessOrEqual(pane2.Height, 0);
			Assert.LessOrEqual(pane2.Width, 0);
		}


		internal class TestDeviceInfo : DeviceInfo
		{
			public TestDeviceInfo()
			{
				CurrentOrientation = DeviceOrientation.Portrait;
			}
			public override Size PixelScreenSize
			{
				get { return new Size(1000, 2000); }
			}

			public override Size ScaledScreenSize
			{
				get { return new Size(500, 1000); }
			}

			public override double ScalingFactor
			{
				get { return 2; }
			}
		}

	}
}
