import { Component, OnInit } from '@angular/core';
import { SearchService } from '../search.service';

@Component({
  selector: 'app-search-page',
  templateUrl: './search-page.component.html',
  styleUrls: ['./search-page.component.css']
})
export class SearchPageComponent implements OnInit {
  query: string = '';
  results: any[] = [];

  constructor(private searchService: SearchService) { }

  ngOnInit() {
    // You might want to load previous searches from a service
  }

  search() {
    this.searchService.search(this.query).subscribe(
      data => this.results = data,
      error => {
        // Handle error
      }
    );
  }
}
