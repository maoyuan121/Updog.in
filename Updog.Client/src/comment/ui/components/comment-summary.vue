<template>
    <div class="py-1">
        <!-- Comment -->
        <div class="d-flex flex-row">
            <comment-vote-controller :comment="comment" v-if="isExpanded" />

            <div class="flex-grow-1">
                <!-- Header -->
                <div class="d-flex flex-row align-items-center">
                    <comment-expand-button @toggle="onExpand" />&nbsp;
                    <user-link :user="comment.user" />&nbsp;
                    <span
                        class="pr-1 grey--text"
                    >{{ comment.karma}} {{comment.karma === 1 ? 'point' : 'points'}}</span>

                    <time-stamp
                        :date="comment.creationDate"
                        :modified="comment.wasUpdated"
                        class="text-muted"
                    />
                </div>

                <!-- Body -->
                <div v-if="isExpanded">
                    <div v-show="!isEditing">{{ comment.body}}</div>
                    <div v-show="isEditing">
                        <v-textarea
                            v-model="editedBody"
                            name="editCommentBody"
                            v-validate="'required|max:10000'"
                            :error="errors.first('editCommentBody') != null"
                            :error-messages="errors.first('editCommentBody')"
                        />
                        <v-btn color="primary" @click="onEditSave">Save</v-btn>
                        <v-btn color="error" outlined @click="onEditCancel">Cancel</v-btn>
                    </div>

                    <!-- Actions -->
                    <div class="d-flex flex-row">
                        <!-- Permalink -->
                        <router-link
                            class="grey--text pl-0 pr-1"
                            :to="{name: 'comment', params: { commentId: comment.id}}"
                        >permalink</router-link>

                        <a
                            href="#"
                            class="grey--text pl-0 pr-1"
                            v-if="canEdit()"
                            @click.prevent="onEditClick"
                        >edit</a>

                        <a
                            href="#"
                            class="grey--text pl-0 pr-1"
                            v-if="canEdit()"
                            @click.prevent="onDeleteClick"
                        >delete</a>

                        <div v-if="isDeleting">
                            <are-you-sure @yes="onDeleteConfirm" @no="onDeleteCancel" />&nbsp;
                        </div>

                        <!-- Reply -->
                        <a
                            href="#"
                            class="primary--text pl-0 pr-1"
                            @click.prevent="onReplyClick"
                        >reply</a>
                    </div>

                    <!-- Reply Box -->
                    <div v-show="isReplying">
                        <comment-create-form
                            ref="commentCreateForm"
                            @submit="onReplySubmit"
                            @cancel="isReplying = false"
                            :parentId="comment.id"
                        />
                    </div>
                </div>
            </div>
        </div>

        <!-- Children -->
        <div v-if="isExpanded">
            <comment-summary
                v-for="child in comment.children"
                :comment="child"
                v-bind:key="child.id"
                class="pl-4"
            />
        </div>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop, Mixins } from 'vue-property-decorator';
import { Comment } from '@/comment/domain/comment';
import CommentUpdaterMixin from '@/comment/mixins/comment-updater-mixin';
import TimeStamp from '@/core/ui/components/time-stamp.vue';
import UserLink from '@/user/ui/components/user-link.vue';
import CommentCreateForm from '@/comment/ui/components/comment-create-form.vue';
import CommentVoteController from '@/vote/ui/components/comment-vote-controller.vue';
import CommentExpandButton from '@/comment/ui/components/comment-expand-button.vue';
import AreYouSure from '@/core/ui/components/are-you-sure.vue';
import { CommentUpdateParams } from '@/comment/interactors/update/comment-update-params';

/**
 * Component to show a comment on screen.
 */
@Component({
    name: 'comment-summary',
    components: {
        UserLink,
        TimeStamp,
        CommentCreateForm,
        CommentVoteController,
        CommentExpandButton,
        AreYouSure
    }
})
export default class CommentSummary extends CommentUpdaterMixin {
    public $refs!: {
        commentCreateForm: CommentCreateForm;
    };

    @Prop()
    public comment!: Comment;

    /**
     * If the comment body is currently expanded.
     */
    public isExpanded: boolean = true;

    /**
     * If the user is currently replying to this comment.
     */
    public isReplying: boolean = false;

    /**
     * If the user is currently editing this comment.
     */
    public isEditing: boolean = false;

    public isDeleting: boolean = false;

    /**
     * The raw text of the editted comment.
     */
    public editedBody: string = '';

    public created() {
        this.$validator.localize('en', {
            custom: {
                editCommentBody: {
                    required: 'Body is required.',
                    max: 'Body may not be longer than 10,000 characters.'
                }
            }
        });
    }

    /**
     * Event handler for when the comment body is expanded / collapsed.
     */
    public onExpand(expanded: boolean) {
        this.isExpanded = expanded;
    }

    /**
     * User clicked on the "reply" button to a comment. Show the reply commen textbox.
     */
    public async onReplyClick() {
        // If the user isn't logged in, send them to the login page.
        if (!this.$isLoggedIn()) {
            this.$redirectToLogin();
        } else {
            this.isReplying = true;

            // Auto focus the textbox to make user's life easier
            this.$nextTick(() => {
                this.$refs.commentCreateForm.focus();
            });
        }
    }

    /**
     * User clicked submit on the reply. Attempt to send it over to the backend.
     */
    public async onReplySubmit(text: string) {
        this.isReplying = false;
    }

    /**
     * Check to see if the user can actually edit this comment.
     */
    public canEdit() {
        // First check to see if they are even logged in.
        if (!this.$isLoggedIn()) {
            return false;
        }

        return this.comment.user.id === this.$login!.user.id && !this.isEditing && !this.isDeleting;
    }

    public onEditClick() {
        this.isEditing = true;
        this.editedBody = this.comment.body;
    }

    public async onEditSave() {
        // Validate first.
        if (!(await this.$validator.validate())) {
            return;
        }

        await this.$updateComment(new CommentUpdateParams(this.comment.id, this.editedBody));
        this.isEditing = false;
    }

    public async onEditCancel() {
        this.isEditing = false;
    }

    public onDeleteClick() {
        this.isDeleting = true;
    }

    public async onDeleteConfirm() {
        await this.$deleteComment(this.comment);
    }

    public async onDeleteCancel() {
        this.isDeleting = false;
    }
}
</script>