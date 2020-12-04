<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;

class CreateLinkVisitsTable extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::create('link_visits', function (Blueprint $table) {
            $table->id();
            $table->foreignId('link_id')->constrained();
            $table->string('platform', 30)->index()->nullable();
            $table->string('device', 30)->index()->nullable();
            $table->string('browser', 30)->index()->nullable();
            $table->string('location', 5)->index()->nullable();
            $table->boolean('crawler')->default(false)->index();
            $table->string('referrer')->nullable();
            $table->string('ip')->nullable();
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
        Schema::dropIfExists('link_visits');
    }
}
