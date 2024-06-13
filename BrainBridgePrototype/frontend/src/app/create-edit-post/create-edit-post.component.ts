import { Component, OnInit } from '@angular/core';
import { PostService } from '../post.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-create-edit-post',
  templateUrl: './create-edit-post.component.html',
  styleUrls: ['./create-edit-post.component.css']
})
export class CreateEditPostComponent implements OnInit {
  post: any = { title: '', content: '' };
  isEdit = false;

  constructor(
    private postService: PostService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    
    if (id === null) {
      console.error('Post ID is null');
      // Handle case where 'id' is null (e.g., display an error message or redirect)
      return;
    }
  
    // Convert 'id' to number
    const postId = +id;
  
    // Check if postId is NaN after conversion
    if (isNaN(postId)) {
      console.error('Post ID is not a valid number');
      // Handle case where 'id' is not a valid number (e.g., display an error message or redirect)
      return;
    }
  
    this.postService.getPostById(postId).subscribe(
      data => {
        this.post = data;
      },
      error => {
        console.error('Error fetching post:', error);
        // Handle error
      }
    );
  }
  

  save() {
    if (this.isEdit) {
      this.postService.updatePost(this.post.id, this.post).subscribe(
        () => this.router.navigate(['/main']),
        error => {
          // Handle error
        }
      );
    } else {
      this.postService.createPost(this.post).subscribe(
        () => this.router.navigate(['/main']),
        error => {
          // Handle error
        }
      );
    }
  }
}
