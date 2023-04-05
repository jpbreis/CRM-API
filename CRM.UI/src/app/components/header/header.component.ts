import { Component } from '@angular/core';
import { } from '@fortawesome/free-brands-svg-icons';
import { faSun, faBell } from '@fortawesome/free-regular-svg-icons';
import { faBars } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent {
  faSun = faSun
  faBell = faBell
  faBars = faBars 
}
