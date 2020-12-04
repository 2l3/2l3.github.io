<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;

class CreateLinksTable extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::create('links', function (Blueprint $table) {
            $table->id();
            $table->string('code', 50)->index()->unique();
            $table->string('url');
            $table->integer('type')->default(1);
            $table->string('title')->nullable();
            $table->text('description')->nullable();
            $table->text('tags')->nullable();
            $table->string('password')->nullable();
            $table->boolean('archived')->default(false);
            $table->boolean('disabled')->default(false);
            $table->unsignedBigInteger('user_id')->nullable();
            $table->foreign('user_id')->references('id')->on('users');
            $table->unsignedBigInteger('domain_id')->nullable();
            $table->foreign('domain_id')->references('id')->on('domains');
            $table->timestamp('expires_at')->nullable();
            $table->timestamps();
        });
    }

    /**
     * Reverse the migrations.
     *
     * @return void
     */
    public function down()
    {
        Schema::dropIfExists('links');
    }
}
