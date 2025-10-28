<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="Admin_Dashboard" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

			<div class="section">
				<div class="box">
					<div class="title">
						Overview
						<span class="hide"></span>
					</div>
					<div class="content">
						<form action="">
							<div class="row">
								<label>Total Sales</label>
								<div class="right"><input type="text" value="" class="onlytext" /></div>
							</div>
							<div class="row">
								<label>Total Sales This Year</label>
								<div class="right"><input type="text" value="" class="onlylow" /></div>
							</div>
							<div class="row">
								<label>Total Orders</label>
								<div class="right"><input type="text" value=""  class="onlyup" /></div>
							</div>
							<div class="row">
								<label>No. of Customers</label>
								<div class="right"><input type="text" value="" class="onlynum" /></div>
							</div>
							<div class="row">
								<label>Customers Awaiting Approval:</label>
								<div class="right"><input type="text" value=""  class="onlyurl" /></div>
							</div>
							<div class="row">
								<label>Reviews Awaiting Approval</label>
								<div class="right"><input type="text" value=""  class="onlyup" /></div>
							</div>
							<div class="row">
								<label>No. of Affiliates</label>
								<div class="right"><input type="text" value=""  class="onlynum" /></div>
							</div>
							<div class="row">
								<label>Affiliates Awaiting Approval</label>
								<div class="right"><input type="text" value=""  class="onlyurl" /></div>
							</div>
						</form>
					</div>
				</div>
			</div>
			
			<div class="section">
				<div class="box">
					<div class="title">
						Latest 10 Orders
						<span class="hide"></span>
					</div>
					<div class="content">
						<table cellspacing="0" cellpadding="0" border="0" class="all"> 
							<thead> 
								<tr>
									<th>Username</th>
									<th>Duration</th>
									<th>Date</th>
									<th>Last visit page</th>
								</tr>
							</thead>
							<tbody>
								<tr>
									<td>John Do</td>
									<td>10 min</td>
									<td>23 January 2012</td>
									<td><a href="#">Dashboard</a></td>
								</tr>
								<tr>
									<td>Hong Gildong</td>
									<td>3 min</td>
									<td>23 January 2012</td>
									<td><a href="#">Login</a></td>
								</tr>
								<tr>
									<td>Israel Israeli</td>
									<td>7 min</td>
									<td>23 January 2012</td>
									<td><a href="#">Our Company</a></td>
								</tr>
								<tr>
									<td>John Smith</td>
									<td>3 hours</td>
									<td>23 January 2012</td>
									<td><a href="#">Message inbox</a></td>
								</tr>
								<tr>
									<td>Luther Blissett</td>
									<td>41 min</td>
									<td>23 January 2012</td>
									<td><a href="#">My profile</a></td>
								</tr>
								<tr>
									<td>Tommy Atkins</td>
									<td>1 hour</td>
									<td>23 January 2012</td>
									<td><a href="#">Settings</a></td>
								</tr>
								<tr>
									<td>Average Joe</td>
									<td>39 min</td>
									<td>23 January 2012</td>
									<td><a href="#">Contact form</a></td>
								</tr>
								<tr>
									<td>Nomen nescio</td>
									<td>56 sec</td>
									<td>23 January 2012</td>
									<td><a href="#">Build a page</a></td>
								</tr>
								<tr>
									<td>Joe Shmoe</td>
									<td>45 min</td>
									<td>23 January 2012</td>
									<td><a href="#">My statics</a></td>
								</tr>
								<tr>
									<td>Jane Doe</td>
									<td>23 min</td>
									<td>23 January 2012</td>
									<td><a href="#">Dashboard</a></td>
								</tr>
								<tr>
									<td>John Do</td>
									<td>10 min</td>
									<td>23 January 2012</td>
									<td><a href="#">Dashboard</a></td>
								</tr>
								<tr>
									<td>Hong Gildong</td>
									<td>3 min</td>
									<td>23 January 2012</td>
									<td><a href="#">Login</a></td>
								</tr>
								<tr>
									<td>Israel Israeli</td>
									<td>7 min</td>
									<td>23 January 2012</td>
									<td><a href="#">Our Company</a></td>
								</tr>
								<tr>
									<td>John Smith</td>
									<td>3 hours</td>
									<td>23 January 2012</td>
									<td><a href="#">Message inbox</a></td>
								</tr>
								<tr>
									<td>Luther Blissett</td>
									<td>41 min</td>
									<td>23 January 2012</td>
									<td><a href="#">My profile</a></td>
								</tr>
								<tr>
									<td>Tommy Atkins</td>
									<td>1 hour</td>
									<td>23 January 2012</td>
									<td><a href="#">Settings</a></td>
								</tr>
								<tr>
									<td>Average Joe</td>
									<td>39 min</td>
									<td>23 January 2012</td>
									<td><a href="#">Contact form</a></td>
								</tr>
								<tr>
									<td>Nomen nescio</td>
									<td>56 sec</td>
									<td>23 January 2012</td>
									<td><a href="#">Build a page</a></td>
								</tr>
								<tr>
									<td>Joe Shmoe</td>
									<td>45 min</td>
									<td>23 January 2012</td>
									<td><a href="#">My statics</a></td>
								</tr>
								<tr>
									<td>Jane Doe</td>
									<td>23 min</td>
									<td>23 January 2012</td>
									<td><a href="#">Dashboard</a></td>
								</tr>
								<tr>
									<td>John Do</td>
									<td>10 min</td>
									<td>23 January 2012</td>
									<td><a href="#">Dashboard</a></td>
								</tr>
								<tr>
									<td>Hong Gildong</td>
									<td>3 min</td>
									<td>23 January 2012</td>
									<td><a href="#">Login</a></td>
								</tr>
								<tr>
									<td>Israel Israeli</td>
									<td>7 min</td>
									<td>23 January 2012</td>
									<td><a href="#">Our Company</a></td>
								</tr>
								<tr>
									<td>John Smith</td>
									<td>3 hours</td>
									<td>23 January 2012</td>
									<td><a href="#">Message inbox</a></td>
								</tr>
								<tr>
									<td>Luther Blissett</td>
									<td>41 min</td>
									<td>23 January 2012</td>
									<td><a href="#">My profile</a></td>
								</tr>
								<tr>
									<td>Tommy Atkins</td>
									<td>1 hour</td>
									<td>23 January 2012</td>
									<td><a href="#">Settings</a></td>
								</tr>
								<tr>
									<td>Average Joe</td>
									<td>39 min</td>
									<td>23 January 2012</td>
									<td><a href="#">Contact form</a></td>
								</tr>
								<tr>
									<td>Nomen nescio</td>
									<td>56 sec</td>
									<td>23 January 2012</td>
									<td><a href="#">Build a page</a></td>
								</tr>
								<tr>
									<td>Joe Shmoe</td>
									<td>45 min</td>
									<td>23 January 2012</td>
									<td><a href="#">My statics</a></td>
								</tr>
								<tr>
									<td>Jane Doe</td>
									<td>23 min</td>
									<td>23 January 2012</td>
									<td><a href="#">Dashboard</a></td>
								</tr>
							</tbody>
						</table>
					</div>
				</div>
			</div>
			
			<div class="section">
				<div class="box">
					<div class="title">
						Statistics
						<span class="hide"></span>
					</div>
					<div class="content">
						<div class="chart-caption">Statistics February</div> 
						<table class="chart" style="width : 100%;"> 
							<thead> 
								<tr> 
									<th></th> 
									<th style="color : #e10000;">unique visitors</th> 
									<th style="color : #000000;">pageviews</th> 
								</tr> 
							</thead> 
							<tbody> 
								<tr> 
									<th>1</th><td>10</td><td>60</td> 
								</tr> 
								<tr> 
									<th>2</th><td>40</td><td>90</td> 
								</tr> 
								<tr> 
									<th>3</th><td>70</td><td>120</td> 
								</tr> 
								<tr> 
									<th>4</th><td>100</td><td>150</td> 
								</tr> 
								<tr> 
									<th>5</th><td>130</td><td>180</td> 
								</tr> 
								<tr> 
									<th>6</th><td>160</td><td>210</td> 
								</tr> 
								<tr> 
									<th>7</th><td>190</td><td>240</td> 
								</tr> 
								<tr> 
									<th>8</th><td>160</td><td>210</td> 
								</tr> 
								<tr> 
									<th>9</th><td>130</td><td>180</td> 
								</tr> 
								<tr> 
									<th>10</th><td>100</td><td>150</td> 
								</tr> 
								<tr> 
									<th>11</th><td>70</td><td>120</td> 
								</tr> 
								<tr> 
									<th>12</th><td>40</td><td>90</td> 
								</tr> 
								<tr> 
									<th>13</th><td>70</td><td>120</td> 
								</tr> 
								<tr> 
									<th>14</th><td>100</td><td>150</td> 
								</tr> 
								<tr> 
									<th>15</th><td>130</td><td>180</td> 
								</tr> 
								<tr> 
									<th>16</th><td>160</td><td>210</td> 
								</tr> 
								<tr> 
									<th>17</th><td>190</td><td>240</td> 
								</tr> 
								<tr> 
									<th>18</th><td>220</td><td>270</td> 
								</tr> 
								<tr> 
									<th>19</th><td>250</td><td>240</td> 
								</tr> 
								<tr> 
									<th>20</th><td>280</td><td>210</td> 
								</tr> 
								<tr> 
									<th>21</th><td>310</td><td>180</td> 
								</tr> 
								<tr> 
									<th>22</th><td>280</td><td>150</td> 
								</tr> 
								<tr> 
									<th>23</th><td>250</td><td>120</td> 
								</tr> 
								<tr> 
									<th>24</th><td>220</td><td>90</td> 
								</tr> 
								<tr> 
									<th>25</th><td>190</td><td>60</td> 
								</tr> 
								<tr> 
									<th>26</th><td>160</td><td>90</td> 
								</tr> 
								<tr> 
									<th>27</th><td>130</td><td>120</td> 
								</tr> 
								<tr> 
									<th>28</th><td>100</td><td>150</td> 
								</tr> 
								<tr> 
									<th>29</th><td>70</td><td>120</td> 
								</tr> 
								<tr> 
									<th>30</th><td>40</td><td>90</td> 
								</tr>
								<tr> 
									<th>31</th><td>10</td><td>60</td> 
								</tr> 
							</tbody> 
						</table>
					</div>
				</div>
			</div>
			
			<div class="section">
				<div class="box">
					<div class="title">
						Orders
						<span class="hide"></span>
					</div>
					<div class="content nopadding">
						<div class="wizard">
							<ul>
								<li><a href="#step-1">Order Details</a></li>
								<li><a href="#step-2">Payment Details</a></li>
								<li><a href="#step-3">Shipping Detials</a></li>
							</ul>
							<div id="step-1">
								<div class="message inner red">
									<span><b>Error</b>: This is a error message</span>
								</div>
								<form action="">
									<div class="row">
										<label>Order ID</label>
										<div class="right"><input type="text" value="" /></div>
									</div>
									<div class="row">
										<label>Invoice No</label>
										<div class="right"><input type="password" value="" /></div>
									</div>
									<div class="row">
										<label>Customer</label>
										<div class="right"><input type="password" value="" /></div>
									</div>
									<div class="row">
										<label>E-mail</label>
										<div class="right"><input type="password" value="" /></div>
									</div>
									<div class="row">
										<label>Telephone</label>
										<div class="right"><input type="password" value="" /></div>
									</div>
									<div class="row">
										<label>Total</label>
										<div class="right"><input type="password" value="" /></div>
									</div>
									<div class="row">
										<label>Order Status</label>
										<div class="right"><input type="password" value="" /></div>
									</div>
									
								</form>
							</div>
							<div id="step-2">
								<table cellspacing="0" cellpadding="0" border="0"> 
									<thead> 
										<tr>
											<th>Username</th>
											<th>Duration</th>
											<th>Date</th>
											<th>Last visit page</th>
										</tr>
									</thead>
									<tbody>
										<tr>
											<td>John Do</td>
											<td>10 min</td>
											<td>23 January 2012</td>
											<td><a href="#">Dashboard</a></td>
										</tr>
										<tr>
											<td>Hong Gildong</td>
											<td>3 min</td>
											<td>23 January 2012</td>
											<td><a href="#">Login</a></td>
										</tr>
										<tr>
											<td>Israel Israeli</td>
											<td>7 min</td>
											<td>23 January 2012</td>
											<td><a href="#">Our Company</a></td>
										</tr>
										<tr>
											<td>John Smith</td>
											<td>3 hours</td>
											<td>23 January 2012</td>
											<td><a href="#">Message inbox</a></td>
										</tr>
										<tr>
											<td>Luther Blissett</td>
											<td>41 min</td>
											<td>23 January 2012</td>
											<td><a href="#">My profile</a></td>
										</tr>
										<tr>
											<td>Tommy Atkins</td>
											<td>1 hour</td>
											<td>23 January 2012</td>
											<td><a href="#">Settings</a></td>
										</tr>
									</tbody>
								</table>
							</div>
							<div id="step-3">
								<h1>A nice title here</h1>
								<p>
									<b>Integer augue lacus, lobortis in luctus in, sodales non libero. Nulla facilisi. Morbi nec est ut eros
									sollicitudin rhoncus. Pellentesque consectetur massa vitae eros varius in ultricies elit laoreet.</b>
								</p>
								<p>
									Mauris aliquet orci mi, at gravida metus. Integer et ante eu augue faucibus dapibus. Pellentesque
									iaculis tempus lorem, id convallis massa ultricies eu. Praesent facilisis nisi est, mattis rutrum sapien.
									Etiam et justo libero, quis aliquam leo. Fusce eros velit, aliquam hendrerit tincidunt quis, laoreet id nisi.
									Sed mauris velit, rutrum eget eleifend at, feugiat sed sem.
								</p>
							</div>
						</div>
					</div>
				</div>
			</div>
		
</asp:Content>

