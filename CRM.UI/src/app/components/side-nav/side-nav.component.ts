import { Component, OnInit } from '@angular/core';
import { faHome, faChartBar, faShoppingBag, faMoneyBill, faCog, faInfoCircle, faEnvelope } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-side-nav',
  templateUrl: './side-nav.component.html',
  styleUrls: ['./side-nav.component.css']
})
export class SideNavComponent implements OnInit {
  faHome = faHome;
  faChartBar = faChartBar;
  faShoppingBag = faShoppingBag;
  faMoneyBill = faMoneyBill;
  faCog = faCog;
  faInfoCircle = faInfoCircle;
  faEnvelope = faEnvelope;
  
  List = [
    {
      name: 'Home',
      icon: 'fa-home'
    },
    {
      name: 'Análises',
      icon: 'fa-chart-bar'
    },
    {
      name: 'Produtos',
      icon: 'fa-shopping-bag'
    },
    {
      name: 'Vendas',
      icon: 'fa-money-bill'
    },
    {
      name: 'Configuração',
      icon: 'fa-cog'
    },
    {
      name: 'Sobre',
      icon: 'fa-info-circle'
    },
    {
      name: 'Contato',
      icon: 'fa-envelope'
    }
  ];

  ngOnInit(): void {};
}
