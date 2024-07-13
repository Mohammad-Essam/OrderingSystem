async function deleteOrder(id)
{
    if(!confirm("Are you sure you want to delete this order?")) return;
    try
    {
        let response = await fetch(`api/Orders/${id}`,{
            method:"DELETE",
        headers:{"authorization":`Bearer ${localStorage.getItem("api_token")}`,"Content-Type":"application/json"},
        });
        if(response.status == 200)
        {
            toastr.success("Order Deleted Successfully");
            document.getElementById(id).remove();
        }
        else
        {
            toastr.error(await response.text());
        }
   }
   catch(err)
   {
       toastr.error("Error Deleting Order");
   }
}