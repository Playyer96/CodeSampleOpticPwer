using System;
using System.Collections.Generic;

namespace DreamHouseStudios.SofasaLogistica {
	[Serializable]
	public class ProductData {
		public string productId;
		public string description;
        public string location;
		public string max;
		public string td;
		public string broken;
		public string shelfId;
	}

	[Serializable]
	public class ShelfData {
		public string shelfId;
	}

	[Serializable]
	public class BoxData {
		public string boxId;
		public List<BoxContent> content;

		public int TotalQuantity {
			get {
				int totalQuantity = 0;
				foreach (BoxContent c in content)
					totalQuantity += c.quantity;

				return totalQuantity;
			}
		}
	}

	[Serializable]
	public class BoxContent {
		public ProductData product;
		public int quantity;
	}
}