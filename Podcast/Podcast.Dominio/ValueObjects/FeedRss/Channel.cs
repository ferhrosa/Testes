﻿using System.Collections.Generic;
using System.Xml.Serialization;

namespace Podcast.Dominio.ValueObjects.FeedRss
{
    public class Channel
	{
		[XmlElement("item")]
		public List<Item> Items { get; set; }
	}
}
