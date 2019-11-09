resource "azurerm_sql_server" "yugawara" {
  name                         = "yugawarasqlserver"
  resource_group_name          = azurerm_resource_group.yugawara.name
  location                     = azurerm_resource_group.yugawara.location
  version                      = "12.0"
  administrator_login          = "4dm1n157r470r"
  administrator_login_password = "H2DyzWQgrEZ>zcYfd"
}

resource "azurerm_sql_database" "yugawara" {
  name                = "yugawarasqldatabase"
  resource_group_name = azurerm_resource_group.yugawara.name
  location            = azurerm_resource_group.yugawara.location
  server_name         = azurerm_sql_server.yugawara.name
}

resource "azurerm_sql_firewall_rule" "yugawara" {
  name                = "YugawaraFirewallRule"
  resource_group_name = azurerm_resource_group.yugawara.name
  server_name         = azurerm_sql_server.yugawara.name
  start_ip_address    = "58.91.45.177"
  end_ip_address      = "58.91.45.177"
}
