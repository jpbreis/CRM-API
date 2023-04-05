import { NgModule } from '@angular/core';
import { FaIconLibrary, FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faAddressBook } from '@fortawesome/free-regular-svg-icons';
import { faMask } from '@fortawesome/free-solid-svg-icons';


@NgModule({
  exports: [FontAwesomeModule]
})
export class IconsModule { 
  constructor(library: FaIconLibrary) {
    library.addIcons(faMask, faAddressBook);
  }
}
