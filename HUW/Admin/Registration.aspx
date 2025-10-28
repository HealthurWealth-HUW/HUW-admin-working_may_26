<%@ Page Title="" Language="VB" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="false" CodeFile="Registration.aspx.vb" Inherits="Registration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
			<div class="section">
				<div class="box">
					<div class="title">
						Inputs and textareas
						<span class="hide"></span>
					</div>
					<div class="content">
						<form action="">
							<div class="row">
								<label>Normal input</label>
								<div class="right"><input type="text" value="" /></div>
							</div>
							<div class="row">
								<label>Password input</label>
								<div class="right"><input type="password" value="" /></div>
							</div>
							<div class="row">
								<label>Placeholder input</label>
								<div class="right"><input type="text" value="" placeholder="Here some text" /></div>
							</div>
							<div class="row">
								<label>Readonly input</label>
								<div class="right"><input type="text" readonly="readonly" value="Here some text" /></div>
							</div>
							<div class="row">
								<label>Max lenght input</label>
								<div class="right"><input type="text" maxlength="25" value="" placeholder="Max. 25 characters..." /></div>
							</div>
							<div class="row">
								<label>Checkboxes</label>
								<div class="right">
									<input type="checkbox" name="" value="" id="Checkbox1" checked="checked" />
									<label for="first-check">Check on</label>
									
									<input type="checkbox" name="" value="" id="Checkbox2" />
									<label for="second-check">Check off</label>
								</div>
							</div>
                            <div class="row">
								<label>Radiobuttons</label>
								<div class="right">
									<input type="radio" name="radiobutton" id="radio1" checked="checked" /> 
									<label for="radio-1">Radio on</label>
									
									<input type="radio" name="radiobutton" id="radio2" /> 
									<label for="radio-2">Radio off</label>
								</div>
							</div>
                            <div class="row">
								<label>Multi select</label>
								<div class="right">
									<select multiple="multiple" size="5" class="multiple">
										<option value="">Option number 1</option>
										<option value="">Option number 2</option>
										<option selected="selected" value="">Option number 3</option>
										<option value="">Option number 4</option>
										<option value="">Option number 5</option>
										<option selected="selected" value="">Option number 6</option>
										<option value="">Option number 7</option>
										<option value="">Option number 8</option>
										<option value="">Option number 9</option>
										<option value="">Option number 10</option>
									</select>
								</div>
							</div>
                            <div class="row">
								<label>Normal selectmenu</label>
								<div class="right">
									<select>
										<option selected="selected" value="">The selected one.</option>
										<option value="">Option number 1</option>
										<option value="">Option number 2</option>
										<option value="">Option number 3</option>
										<option value="">Option number 4</option>
										<option value="">Option number 5</option>
									</select>
								</div>
							</div>
                            <div class="row">
								<label>File upload</label>
								<div class="right"><input type="file" style="height:25px;" /></div>
							</div>
                            <div class="row">
								<label>Textarea</label>
								<div class="right"><textarea rows="" cols="" style="height : 100px;"></textarea></div>
							</div>
                            <div class="row">
								<label>Buttons</label>
								<div class="right">
								<button type="submit" class="orange"><span>Sumbit</span></button>
								</div>
							</div>
						</form>
					</div>
				</div>
			</div>			
</asp:Content>

