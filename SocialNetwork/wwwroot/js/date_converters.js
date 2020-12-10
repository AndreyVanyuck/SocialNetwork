const items = document.getElementsByClassName("utc-date");
for (let i = 0; i < items.length; i++) {
	let date = new Date(items[i].innerHTML);
	let newDate = new Date(Date.UTC(date.getUTCFullYear(), date.getUTCMonth(), date.getUTCDate(), date.getHours(), date.getMinutes(), date.getSeconds()));
	items[i].innerHTML = newDate.getDate() + "." + newDate.getMonth() + "." + newDate.getFullYear() + " " + newDate.getHours() + ":" + newDate.getMinutes() + ":" + newDate.getSeconds();
}



